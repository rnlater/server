using Application.Interfaces;
using Application.UseCases.Auth;
using AutoMapper;
using Endpoint.ApiRequests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IJWTService jWTService, IMapper mapper)
        {
            _authService = authService;
            _jWTService = jWTService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginRequest, LoginParams>();
                cfg.CreateMap<RegisterRequest, RegisterParams>();
                cfg.CreateMap<ConfirmRegistrationEmailRequest, ConfirmRegistrationEmailParams>();
                cfg.CreateMap<ForgotPasswordRequest, ForgotPasswordParams>();
                cfg.CreateMap<ConfirmPasswordResettingEmailRequest, ConfirmPasswordResettingEmailParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var Params = _mapper.Map<LoginParams>(request);

            var LoginResult = await _authService.Login(Params);

            if (LoginResult.IsSuccess)
            {
                var TokenPairResult = await _jWTService.GenerateTokenPair(LoginResult.Value);

                return TokenPairResult.IsSuccess
                    ? Ok(new
                    {
                        AccessToken = TokenPairResult.Value.Item1,
                        RefreshToken = TokenPairResult.Value.Item2
                    })
                    : BadRequest(TokenPairResult.Errors);
            }
            return BadRequest(LoginResult.Errors);
        }

        [HttpPost(HttpRoute.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var Params = _mapper.Map<RegisterParams>(request);
            var RegisterResult = await _authService.Register(Params);
            return RegisterResult.IsSuccess ? Ok(RegisterResult.Value) : BadRequest(RegisterResult.Errors);
        }

        [HttpPost(HttpRoute.ConfirmRegistrationEmail)]
        public async Task<IActionResult> ConfirmRegistrationEmail([FromBody] ConfirmRegistrationEmailRequest request)
        {
            var Params = _mapper.Map<ConfirmRegistrationEmailParams>(request);
            var ConfirmEmailResult = await _authService.ConfirmRegistrationEmail(Params);
            if (ConfirmEmailResult.IsSuccess)
            {
                var TokenPairResult = await _jWTService.GenerateTokenPair(ConfirmEmailResult.Value);

                return TokenPairResult.IsSuccess
                    ? Ok(new
                    {
                        AccessToken = TokenPairResult.Value.Item1,
                        RefreshToken = TokenPairResult.Value.Item2
                    })
                    : BadRequest(TokenPairResult.Errors);
            }
            return BadRequest(ConfirmEmailResult.Errors);
        }

        [HttpPost(HttpRoute.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var Params = _mapper.Map<ForgotPasswordParams>(request);
            var ForgotPasswordResult = await _authService.ForgotPassword(Params);
            return ForgotPasswordResult.IsSuccess ? Ok(ForgotPasswordResult.Value) : BadRequest(ForgotPasswordResult.Errors);
        }

        [HttpPost(HttpRoute.ConfirmPasswordResettingEmail)]
        public async Task<IActionResult> ConfirmPasswordResettingEmail([FromBody] ConfirmPasswordResettingEmailRequest request)
        {
            var Params = _mapper.Map<ConfirmPasswordResettingEmailParams>(request);
            var ConfirmPasswordResettingEmailResult = await _authService.ConfirmPasswordResettingEmail(Params);
            if (ConfirmPasswordResettingEmailResult.IsSuccess)
            {
                var TokenPairResult = await _jWTService.GenerateTokenPair(ConfirmPasswordResettingEmailResult.Value);

                return TokenPairResult.IsSuccess
                    ? Ok(new
                    {
                        AccessToken = TokenPairResult.Value.Item1,
                        RefreshToken = TokenPairResult.Value.Item2
                    })
                    : BadRequest(TokenPairResult.Errors);
            }
            return BadRequest(ConfirmPasswordResettingEmailResult.Errors);
        }

        [HttpPost(HttpRoute.Logout)]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var LogoutResult = await _authService.Logout();
            return LogoutResult.IsSuccess ? Ok(LogoutResult.Value) : BadRequest(LogoutResult.Errors);
        }
    }
}
