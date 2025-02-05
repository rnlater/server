using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.PublicationRequests
{
    public class UpdateKnowledgeVisibilityParams
    {
        public Guid KnowledgeId { get; set; }
        public KnowledgeVisibility Visibility { get; set; }
    }
    public class UpdateKnowledgeVisibilityUseCase : IUseCase<KnowledgeDto, UpdateKnowledgeVisibilityParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateKnowledgeVisibilityUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeDto>> Execute(UpdateKnowledgeVisibilityParams parameters)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(
                    new BaseSpecification<Knowledge>(k => k.Id == parameters.KnowledgeId)
                    .AddInclude(query => query.Include(k => k.PublicationRequest!)));

                if (knowledge == null)
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);

                if (knowledge.Visibility == parameters.Visibility)
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoChangeDetected);

                if (knowledge.PublicationRequest != null)
                {
                    if (parameters.Visibility == KnowledgeVisibility.Public)
                        knowledge.PublicationRequest.Status = PublicationRequestStatus.Approved;
                    else
                        knowledge.PublicationRequest.Status = PublicationRequestStatus.Rejected;

                    knowledge.PublicationRequest = await _unitOfWork.Repository<PublicationRequest>().Update(knowledge.PublicationRequest);
                }
                knowledge.Visibility = parameters.Visibility;

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