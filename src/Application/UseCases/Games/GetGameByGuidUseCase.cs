using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class GetGameByGuidUseCase : IUseCase<GameDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGameByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GameDto>> Execute(Guid id)
        {
            try
            {
                var gameRepository = _unitOfWork.Repository<Game>();
                var game = await gameRepository.Find(new BaseSpecification<Game>(g => g.Id == id));

                if (game == null)
                {
                    return Result<GameDto>.Fail(ErrorMessage.NoGameFoundWithGuid);
                }

                return Result<GameDto>.Done(_mapper.Map<GameDto>(game));
            }
            catch (Exception)
            {
                return Result<GameDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}