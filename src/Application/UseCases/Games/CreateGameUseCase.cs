using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class CreateGameParams
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IFormFile Image { get; set; }
    }

    public class CreateGameUseCase : IUseCase<GameDto, CreateGameParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public CreateGameUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<GameDto>> Execute(CreateGameParams parameters)
        {
            try
            {
                var gameRepository = _unitOfWork.Repository<Game>();

                var imageStoredResult = await _fileStorageService.StoreFile(parameters.Image, "games");

                if (!imageStoredResult.IsSuccess)
                {
                    return Result<GameDto>.Fail(imageStoredResult.Error);
                }

                var newGame = new Game
                {
                    Name = parameters.Name,
                    Description = parameters.Description,
                    ImageUrl = imageStoredResult.Value
                };

                newGame = await gameRepository.Add(newGame);

                return Result<GameDto>.Done(_mapper.Map<GameDto>(newGame));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<GameDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}