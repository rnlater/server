using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class GetAllGamesUseCase : IUseCase<IEnumerable<GameDto>, NoParam>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGamesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<GameDto>>> Execute(NoParam nothing)
        {
            try
            {
                var gameRepository = _unitOfWork.Repository<Game>();
                var games = await gameRepository.FindMany(new BaseSpecification<Game>());

                if (!games.Any())
                {
                    return Result<IEnumerable<GameDto>>.Fail(ErrorMessage.NoGamesFound);
                }

                return Result<IEnumerable<GameDto>>.Done(_mapper.Map<IEnumerable<GameDto>>(games));
            }
            catch (Exception)
            {
                return Result<IEnumerable<GameDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}