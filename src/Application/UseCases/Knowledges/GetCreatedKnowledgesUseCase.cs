using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges
{
    public class GetCreatedKnowledgesParams
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCreatedKnowledgesUseCase : IUseCase<IEnumerable<KnowledgeDto>, GetCreatedKnowledgesParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCreatedKnowledgesUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IEnumerable<KnowledgeDto>>> Execute(GetCreatedKnowledgesParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<IEnumerable<KnowledgeDto>>.Fail(ErrorMessage.UserNotFound);

                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();

                var specification = new BaseSpecification<Knowledge>(k =>
                    k.CreatorId == userId
                    && (string.IsNullOrEmpty(parameters.Search) || k.Title.Contains(parameters.Search))
                ).AddInclude(query => query.Include(k => k.PublicationRequest!));

                var knowledgeCount = await knowledgeRepository.Count(specification);

                var knowledges = await knowledgeRepository.FindMany(
                    specification.ApplyPaging(parameters.Page, parameters.PageSize));

                if (!knowledges.Any())
                    return Result<IEnumerable<KnowledgeDto>>.Fail(ErrorMessage.NoKnowledgesFound);

                return Result<IEnumerable<KnowledgeDto>>.Done(knowledges.Select(_mapper.Map<KnowledgeDto>), new Paging(parameters.Page, parameters.PageSize, knowledgeCount));
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
