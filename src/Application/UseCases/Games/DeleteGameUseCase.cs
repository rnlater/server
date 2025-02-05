using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class DeleteGameUseCase : IUseCase<GameDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public DeleteGameUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
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

                game = await gameRepository.Delete(id);

                if (game != null && !string.IsNullOrEmpty(game.ImageUrl))
                {
                    var deleteFileResult = _fileStorageService.DeleteFile(game.ImageUrl);
                    if (!deleteFileResult.IsSuccess)
                    {
                        return Result<GameDto>.Fail(deleteFileResult.Error);
                    }
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