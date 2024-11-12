using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Enums;
using Domain.Base;
using Shared.Constants;

namespace Application.UseCases.Knowledges
{
    public class UpdateKnowledgeParams
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public KnowledgeLevel Level { get; set; }
    }

    public class UpdateKnowledgeUseCase : IUseCase<KnowledgeDto, UpdateKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeDto>> Execute(UpdateKnowledgeParams parameters)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == parameters.Id));

                if (knowledge == null)
                {
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                knowledge.Title = parameters.Title;
                knowledge.Level = parameters.Level;

                knowledge = await knowledgeRepository.Update(knowledge);

                return Result<KnowledgeDto>.Done(_mapper.Map<KnowledgeDto>(knowledge));
            }
            catch (Exception)
            {
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}