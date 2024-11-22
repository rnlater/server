using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.PublicationRequests
{
    public class DeletePublicationRequestUseCase : IUseCase<PublicationRequestDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeletePublicationRequestUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PublicationRequestDto>> Execute(Guid guid)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                {
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.UserNotFound);
                }

                var publicationRequest = await _unitOfWork.Repository<PublicationRequest>().Find(
                    new BaseSpecification<PublicationRequest>(pr => pr.Id == guid)
                        .AddInclude(query => query.Include(pr => pr.Knowledge!)
                ));

                if (publicationRequest == null)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.NoPublicationRequestFoundWithGuid);
                else if (publicationRequest.Knowledge!.CreatorId != user.Id)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.UserNotAuthorized);
                else if (publicationRequest.Status == PublicationRequestStatus.Approved)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.PublicationRequestAlreadyApproved);

                publicationRequest = await _unitOfWork.Repository<PublicationRequest>().Delete(publicationRequest.Id);

                return Result<PublicationRequestDto>.Done(_mapper.Map<PublicationRequestDto>(publicationRequest));
            }
            catch (Exception)
            {
                return Result<PublicationRequestDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}