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
    }

    public class CreateKnowledgeParams
    {
        public required string Title { get; set; }
        public KnowledgeLevel Level { get; set; }
        public List<Guid> KnowledgeTypeIds { get; set; } = [];
        public List<Guid> KnowledgeTopicIds { get; set; } = [];
        public List<Guid> SubjectIds { get; set; } = [];
        public List<CreateMaterialParams> Materials { get; set; } = [];
    }

    public class CreateKnowledgeUseCase : IUseCase<KnowledgeDto, CreateKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<KnowledgeDto>> Execute(CreateKnowledgeParams parameters)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeTypeKnowledgeRepository = _unitOfWork.Repository<KnowledgeTypeKnowledge>();
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopicKnowledgeRepository = _unitOfWork.Repository<KnowledgeTopicKnowledge>();
                var subjectRepository = _unitOfWork.Repository<Subject>();

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

                var knowledge = await knowledgeRepository.Add(new Knowledge
                {
                    Id = Guid.NewGuid(),
                    Title = parameters.Title,
                    Level = parameters.Level,
                    Visibility = KnowledgeVisibility.Private,
                });

                if (user.IsAdmin)
                    knowledge.CreatorId = GuidConstants.Admin;
                else
                    knowledge.CreatorId = user.Id;

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
                    knowledge.SubjectKnowledges.Add(new SubjectKnowledge
                    {
                        KnowledgeId = knowledge.Id,
                        SubjectId = subject.Id
                    });
                }

                await CreateMaterialParams.AddMaterialsRecursively(
                    parameters.Materials,
                    knowledge.Id,
                    null,
                    _unitOfWork.Repository<Material>().Add
                );

                return Result<KnowledgeDto>.Done(_mapper.Map<KnowledgeDto>(knowledge));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }

    }

}
