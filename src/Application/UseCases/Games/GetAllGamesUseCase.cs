using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
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
        private readonly IRedisCache _cache;

        public GetAllGamesUseCase(IUnitOfWork unitOfWork, IMapper mapper, IRedisCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<GameDto>>> Execute(NoParam nothing)
        {
            try
            {
                var gameRepository = _unitOfWork.Repository<Game>();

                var gameDtos = await _cache.GetAsync<IEnumerable<GameDto>>($"{RedisCache.Keys.GetAllGames}");

                if (gameDtos == null)
                {
                    var games = await gameRepository.GetAll();
                    gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
                    await _cache.SetAsync($"{RedisCache.Keys.GetAllGames}", gameDtos);
                }

                if (!gameDtos.Any())
                {
                    return Result<IEnumerable<GameDto>>.Fail(ErrorMessage.NoGamesFound);
                }

                return Result<IEnumerable<GameDto>>.Done(gameDtos);
            }
            catch (Exception)
            {
                return Result<IEnumerable<GameDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}