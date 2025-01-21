using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Enums;
using Shared.Constants;
using Domain.Entities.PivotEntities;
using Domain.Base;
using Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.SingleIdPivotEntities;
using Application.Interfaces;

namespace Application.UseCases.Knowledges
{
    public class CreateMaterialParams
    {
        public MaterialType Type { get; set; }
        public required string Content { get; set; }
        public int? Order { get; set; }
        public List<CreateMaterialParams> Children { get; set; } = [];

        public static async Task AddMaterialsRecursively(List<CreateMaterialParams> materials, Guid knowledgeId, Guid? parentId, Func<Material, Task> addMaterial)
        {
            foreach (var materialParams in materials)
            {
                var material = new Material
                {
                    Id = Guid.NewGuid(),
                    Type = materialParams.Type,
                    Content = materialParams.Content,
                    KnowledgeId = knowledgeId,
                    ParentId = parentId,
                    Order = materialParams.Order
                };

                await addMaterial(material);

                if (materialParams.Children.Count != 0)
                {
                    await AddMaterialsRecursively(materialParams.Children, knowledgeId, material.Id, addMaterial);
                }
            }
        }

        public bool ContainsInterpretation()
        {
            if (Type == MaterialType.Interpretation)
                return true;

            return Children.Any(child => child.ContainsInterpretation());
        }
    }

    public class CreateKnowledgeParams
    {
        public required string Title { get; set; }
        public KnowledgeLevel Level { get; set; }
        public List<Guid> KnowledgeTypeIds { get; set; } = [];
        public List<Guid> KnowledgeTopicIds { get; set; } = [];
        public List<Guid> SubjectIds { get; set; } = [];
        public List<CreateMaterialParams> Materials { get; set; } = [];
        public IFormFile? Audio { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Video { get; set; }

        public bool AnyInterpretation => Materials.Any(m => m.ContainsInterpretation());
    }

    public class CreateKnowledgeUseCase : IUseCase<KnowledgeDto, CreateKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileStorageService _fileStorageService;

        public CreateKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<KnowledgeDto>> Execute(CreateKnowledgeParams parameters)
        {
            try
            {
                #region Repositories
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeTypeKnowledgeRepository = _unitOfWork.Repository<KnowledgeTypeKnowledge>();
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopicKnowledgeRepository = _unitOfWork.Repository<KnowledgeTopicKnowledge>();
                var subjectRepository = _unitOfWork.Repository<Subject>();
                var subjectKnowledgeRepository = _unitOfWork.Repository<SubjectKnowledge>();
                var gameKnowledgeSubscriptionRepository = _unitOfWork.Repository<GameKnowledgeSubscription>();
                var gameOptionRepository = _unitOfWork.Repository<GameOption>();
                var gameRepository = _unitOfWork.Repository<Game>();
                #endregion

                #region Validation
                var knowledgeTypes = await knowledgeTypeRepository.FindMany(new BaseSpecification<KnowledgeType>(kt => parameters.KnowledgeTypeIds.Contains(kt.Id)));
                if (knowledgeTypes.Count() != parameters.KnowledgeTypeIds.Count) return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeTypesFound);

                var knowledgeTopics = await knowledgeTopicRepository.FindMany(new BaseSpecification<KnowledgeTopic>(kt => parameters.KnowledgeTopicIds.Contains(kt.Id)));
                if (knowledgeTopics.Count() != parameters.KnowledgeTopicIds.Count) return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeTopicsFound);

                var subjects = await subjectRepository.FindMany(new BaseSpecification<Subject>(s => parameters.SubjectIds.Contains(s.Id)));
                if (subjects.Count() != parameters.SubjectIds.Count) return Result<KnowledgeDto>.Fail(ErrorMessage.NoSubjectsFound);

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<KnowledgeDto>.Fail(ErrorMessage.UserNotFound);

                if (!user.IsAdmin && subjects.Any())
                {
                    return Result<KnowledgeDto>.Fail(ErrorMessage.UserNotAuthorized);
                }

                if (!parameters.Materials.Any(e => e.ContainsInterpretation()))
                {
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoInterpretationForKnowledge);
                }
                #endregion

                #region Knowledge Creation
                var knowledge = await knowledgeRepository.Add(new Knowledge
                {
                    Id = Guid.NewGuid(),
                    Title = parameters.Title,
                    Level = parameters.Level,
                    Visibility = KnowledgeVisibility.Private,
                    CreatorId = user.IsAdmin ? GuidConstants.Admin : user.Id,
                });

                foreach (var knowledgeType in knowledgeTypes)
                {
                    await knowledgeTypeKnowledgeRepository.Add(new KnowledgeTypeKnowledge
                    {
                        KnowledgeId = knowledge.Id,
                        KnowledgeTypeId = knowledgeType.Id
                    });
                }

                foreach (var knowledgeTopic in knowledgeTopics)
                {
                    await knowledgeTopicKnowledgeRepository.Add(new KnowledgeTopicKnowledge
                    {
                        KnowledgeId = knowledge.Id,
                        KnowledgeTopicId = knowledgeTopic.Id
                    });
                }

                foreach (var subject in subjects)
                {
                    await subjectKnowledgeRepository.Add(new SubjectKnowledge
                    {
                        KnowledgeId = knowledge.Id,
                        SubjectId = subject.Id
                    });
                }

                if (parameters.Audio != null)
                {
                    var result = await _fileStorageService.StoreFile(parameters.Audio, "materials/audio");
                    if (result.IsSuccess)
                    {
                        parameters.Materials.Add(new CreateMaterialParams
                        {
                            Type = MaterialType.Audio,
                            Content = result.Value,
                        });
                    }
                }

                if (parameters.Image != null)
                {
                    var result = await _fileStorageService.StoreFile(parameters.Image, "materials/images");
                    if (result.IsSuccess)
                    {
                        parameters.Materials.Add(new CreateMaterialParams
                        {
                            Type = MaterialType.Image,
                            Content = result.Value,
                        });
                    }
                }

                if (parameters.Video != null)
                {
                    var result = await _fileStorageService.StoreFile(parameters.Video, "materials/videos");
                    if (result.IsSuccess)
                    {
                        parameters.Materials.Add(new CreateMaterialParams
                        {
                            Type = MaterialType.Video,
                            Content = result.Value,
                        });
                    }
                }

                await CreateMaterialParams.AddMaterialsRecursively(
                    parameters.Materials,
                    knowledge.Id,
                    null,
                    _unitOfWork.Repository<Material>().Add
                );
                #endregion

                knowledge = await knowledgeRepository.Find(
                    new BaseSpecification<Knowledge>(k => k.Id == knowledge.Id)
                    .AddInclude(query => query.Include(k => k.Materials))
                );
                var knowledgeDto = _mapper.Map<KnowledgeDto>(knowledge);

                #region Choose Correct Answer Games
                var chooseTheCorrectAnswerGame = await gameRepository.Find(new BaseSpecification<Game>(g => g.Name == Shared.Constants.Games.ChooseTheCorrectAnswer));
                if (chooseTheCorrectAnswerGame != null)
                {
                    var threeOthers = knowledgeTopicKnowledgeRepository.FindMany(
                    new BaseSpecification<KnowledgeTopicKnowledge>(
                        ktk => parameters.KnowledgeTopicIds.Contains(ktk.KnowledgeTopicId)
                        && ktk.KnowledgeId != knowledge!.Id
                        && ktk.Knowledge!.Visibility == KnowledgeVisibility.Public
                    )
                    .AddInclude(query => query.Include(ktk => ktk.Knowledge!).ThenInclude(k => k.Materials))
                    .ApplyPaging(1, 3)).Result.Select(ktk => ktk.Knowledge);

                    if (threeOthers.Count() != 3)
                        threeOthers = knowledgeTypeKnowledgeRepository.FindMany(
                            new BaseSpecification<KnowledgeTypeKnowledge>(
                                ktk => parameters.KnowledgeTypeIds.Contains(ktk.KnowledgeTypeId)
                                && ktk.KnowledgeId != knowledge!.Id
                                && ktk.Knowledge!.Visibility == KnowledgeVisibility.Public
                            )
                            .AddInclude(query => query.Include(ktk => ktk.Knowledge!).ThenInclude(k => k.Materials))
                            .ApplyPaging(1, 3)).Result.Select(ktk => ktk.Knowledge);

                    if (threeOthers.Count() != 3)
                        threeOthers = await knowledgeRepository.FindMany(
                            new BaseSpecification<Knowledge>(
                                k => k.Id != knowledge!.Id
                                && k.Visibility == KnowledgeVisibility.Public
                            )
                            .AddInclude(query => query.Include(k => k.Materials))
                            .ApplyPaging(1, 3));

                    List<string> definitions =
                    [
                        knowledgeDto.DistinctInterpretation,
                    .. _mapper.Map<List<KnowledgeDto>>(threeOthers).Select(k => k!.DistinctInterpretation),
                ];
                    List<string> titles = [
                        knowledgeDto.Title,
                    .. _mapper.Map<List<Knowledge>>(threeOthers).Select(k => k!.Title),
                ];

                    var gameKnowledgeSubscription = await gameKnowledgeSubscriptionRepository.Add(new GameKnowledgeSubscription
                    {
                        GameId = chooseTheCorrectAnswerGame.Id,
                        KnowledgeId = knowledge!.Id,
                    });

                    var Group = 1;
                    await gameOptionRepository.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = knowledge.Title,
                        Type = GameOptionType.Question,
                        Group = Group,
                    });
                    int Order = 0;
                    foreach (var definition in definitions)
                    {
                        await gameOptionRepository.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = definition,
                            Type = GameOptionType.Answer,
                            IsCorrect = definition == definitions[0],
                            Group = Group,
                            Order = Order++,
                        });
                    }
                    Group += 1;

                    await gameOptionRepository.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = definitions[0],
                        Type = GameOptionType.Question,
                        Group = Group,
                    });
                    Order = 0;
                    foreach (var title in titles)
                    {
                        await gameOptionRepository.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = title,
                            Type = GameOptionType.Answer,
                            IsCorrect = title == knowledge.Title,
                            Group = Group,
                            Order = Order++,
                        });
                    }
                }
                #endregion

                #region Fill In The Blank Games
                var FillInTheBlankGame = await gameRepository.Find(new BaseSpecification<Game>(g => g.Name == Shared.Constants.Games.FillInTheBlank));
                if (FillInTheBlankGame != null)
                {
                    var gameKnowledgeSubscription = await gameKnowledgeSubscriptionRepository.Add(new GameKnowledgeSubscription
                    {
                        GameId = FillInTheBlankGame.Id,
                        KnowledgeId = knowledge!.Id,
                    });

                    await gameOptionRepository.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = StringTransformer.GetBlankedVersion(knowledge.Title),
                        Type = GameOptionType.Question,
                        Group = 1,
                    });
                    await gameOptionRepository.Add(new GameOption
                    {
                        GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                        Value = knowledge.Title,
                        Type = GameOptionType.Answer,
                        IsCorrect = true,
                        Group = 1,
                    });
                }
                #endregion

                #region Arrange Words Games
                var arrangeWordsGame = await gameRepository.Find(new BaseSpecification<Game>(g => g.Name == Shared.Constants.Games.ArrangeWordsLetters));
                if (arrangeWordsGame != null)
                {
                    var shuffledTitle = StringTransformer.GetShuffledVersion(knowledge!.Title);

                    if (shuffledTitle != null)
                    {
                        var gameKnowledgeSubscription = await gameKnowledgeSubscriptionRepository.Add(new GameKnowledgeSubscription
                        {
                            GameId = arrangeWordsGame.Id,
                            KnowledgeId = knowledge.Id,
                        });

                        await gameOptionRepository.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = shuffledTitle,
                            Type = GameOptionType.Question,
                        });

                        await gameOptionRepository.Add(new GameOption
                        {
                            GameKnowledgeSubscriptionId = gameKnowledgeSubscription.Id,
                            Value = knowledge.Title,
                            Type = GameOptionType.Answer,
                            IsCorrect = true,
                        });
                    }
                }
                #endregion

                return Result<KnowledgeDto>.Done(knowledgeDto);
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
