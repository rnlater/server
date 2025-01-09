using Application.DTOs;
using Application.UseCases.Profile;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly GetProfileUseCase _getProfileUseCase;
        private readonly UpdateProfileUseCase _updateProfileUseCase;

        public ProfileService(GetProfileUseCase getProfileUseCase, UpdateProfileUseCase updateProfileUseCase)
        {
            _getProfileUseCase = getProfileUseCase;
            _updateProfileUseCase = updateProfileUseCase;
        }

        public Task<Result<UserDto>> GetProfile()
        {
            return _getProfileUseCase.Execute(NoParam.Value);
        }

        public Task<Result<UserDto>> UpdateProfile(UpdateProfileParams Params)
        {
            return _updateProfileUseCase.Execute(Params);
        }
    }
}