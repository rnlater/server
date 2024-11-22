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
    public class GetPublicationRequestsParams
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PublicationRequestStatus? Status { get; set; }
    }

    public class GetPublicationRequestsUseCase : IUseCase<IEnumerable<PublicationRequestDto>, GetPublicationRequestsParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPublicationRequestsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PublicationRequestDto>>> Execute(GetPublicationRequestsParams parameters)
        {
            try
            {
                var publicationRequestRepository = _unitOfWork.Repository<PublicationRequest>();
                var specification =
                    new BaseSpecification<PublicationRequest>(pr =>
                        (string.IsNullOrEmpty(parameters.SearchTerm) || pr.Knowledge!.Title.Contains(parameters.SearchTerm))
                            && (!parameters.Status.HasValue || pr.Status == parameters.Status))
                    .AddInclude(query => query
                        .Include(pr => pr.Knowledge!)
                        .ThenInclude(k => k.Creator!)
                        .Include(pr => pr.Knowledge!)
                        .ThenInclude(k => k.Materials))
                    .ApplyPaging(parameters.Page, parameters.PageSize);

                var publicationRequests = await publicationRequestRepository.FindMany(specification);
                if (!publicationRequests.Any())
                    return Result<IEnumerable<PublicationRequestDto>>.Fail(ErrorMessage.NoPublicationRequestsFound);

                return Result<IEnumerable<PublicationRequestDto>>.Done(_mapper.Map<IEnumerable<PublicationRequestDto>>(publicationRequests));
            }
            catch (Exception)
            {
                return Result<IEnumerable<PublicationRequestDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}