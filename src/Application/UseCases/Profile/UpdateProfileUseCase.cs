using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Profile
{
    public class UpdateProfileParams
    {
        public required string UserName { get; set; }
        public IFormFile? Photo { get; set; }
    }

    public class UpdateProfileUseCase : IUseCase<UserDto, UpdateProfileParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileStorageService _fileStorageService;

        public UpdateProfileUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<UserDto>> Execute(UpdateProfileParams parameters)
        {
            try
            {
                var userRepository = _unitOfWork.Repository<User>();

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await userRepository.GetById(userId.Value);
                if (user == null)
                    return Result<UserDto>.Fail(ErrorMessage.UserNotFound);

                if (parameters.Photo != null)
                {
                    if (!string.IsNullOrEmpty(user.PhotoUrl))
                    {
                        Result<string> deleteFileResult = _fileStorageService.DeleteFile(user.PhotoUrl);

                        if (!deleteFileResult.IsSuccess)
                        {
                            return Result<UserDto>.Fail(deleteFileResult.Error);
                        }
                    }

                    var photoStoredResult = await _fileStorageService.StoreFile(parameters.Photo, "users");
                    if (!photoStoredResult.IsSuccess)
                    {
                        return Result<UserDto>.Fail(photoStoredResult.Error);
                    }

                    user.PhotoUrl = photoStoredResult.Value;
                }
                user.UserName = parameters.UserName;
                user = await userRepository.Update(user);

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Done(userDto);
            }
            catch (Exception)
            {
                return Result<UserDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}