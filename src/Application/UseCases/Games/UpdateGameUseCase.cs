using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class UpdateGameParams
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class UpdateGameUseCase : IUseCase<GameDto, UpdateGameParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public UpdateGameUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<GameDto>> Execute(UpdateGameParams parameters)
        {
            try
            {
                var gameRepository = _unitOfWork.Repository<Game>();
                var game = await gameRepository.Find(new BaseSpecification<Game>(g => g.Id == parameters.Id));

                if (game == null)
                {
                    return Result<GameDto>.Fail(ErrorMessage.NoGameFoundWithGuid);
                }

                if (parameters.Image != null)
                {
                    if (!string.IsNullOrEmpty(game.ImageUrl))
                    {
                        var deleteFileResult = _fileStorageService.DeleteFile(game.ImageUrl);

                        if (!deleteFileResult.IsSuccess)
                        {
                            return Result<GameDto>.Fail(deleteFileResult.Error);
                        }
                    }

                    var imageStoredResult = await _fileStorageService.StoreFile(parameters.Image, "games");

                    if (!imageStoredResult.IsSuccess)
                    {
                        return Result<GameDto>.Fail(imageStoredResult.Error);
                    }

                    game.ImageUrl = imageStoredResult.Value;
                }

                game.Name = parameters.Name;
                game.Description = parameters.Description;

                game = await gameRepository.Update(game);

                return Result<GameDto>.Done(_mapper.Map<GameDto>(game));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<GameDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}