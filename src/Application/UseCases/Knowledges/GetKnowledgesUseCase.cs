using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges
{
    public class GetKnowledgesParams
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetKnowledgesUseCase : IUseCase<IEnumerable<KnowledgeDto>, GetKnowledgesParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKnowledgesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<KnowledgeDto>>> Execute(GetKnowledgesParams parameters)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();

                var specification = new BaseSpecification<Knowledge>(
                    k => string.IsNullOrEmpty(parameters.Search) || k.Title.Contains(parameters.Search)
                )
                .AddInclude(query => query.Include(k => k.Materials))
                .ApplyPaging(parameters.Page, parameters.PageSize);

                var knowledges = await knowledgeRepository.FindMany(specification);

                if (!knowledges.Any())
                {
                    return Result<IEnumerable<KnowledgeDto>>.Fail(ErrorMessage.NoKnowledgesFound);
                }
                var knowledgeCount = await knowledgeRepository.Count();

                return Result<IEnumerable<KnowledgeDto>>.Done(knowledges.Select(_mapper.Map<KnowledgeDto>), new Paging(parameters.Page, parameters.PageSize, knowledgeCount));
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
