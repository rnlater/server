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
using System.Security.Claims;

namespace Application.UseCases.Subjects;

public class GetSubjectByGuidUseCase : IUseCase<SubjectDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetSubjectByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<SubjectDto>> Execute(Guid id)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();
            var subject = await subjectRepository.Find(
                new BaseSpecification<Subject>(s => s.Id == id)
                .AddInclude(query => query
                    .Include(s => s.TrackSubjects)
                    .ThenInclude(ts => ts.Track!)
                    .Include(s => s.SubjectKnowledges)
                    .ThenInclude(sk => sk.Knowledge!)));

            if (subject == null)
            {
                return Result<SubjectDto>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
            }

            var subjectDto = _mapper.Map<SubjectDto>(subject);

            var user = _httpContextAccessor.HttpContext.User;
            if (user.IsInRole(Role.User.ToString()))
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Result<SubjectDto>.Fail(ErrorMessage.UserNotFound);
                }
                var userId = Guid.Parse(userIdClaim);

                // TODO
            }

            return Result<SubjectDto>.Done(subjectDto);
        }
        catch (Exception)
        {
            return Result<SubjectDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}