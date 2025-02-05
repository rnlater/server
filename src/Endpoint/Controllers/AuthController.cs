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
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LoginRequest, LoginParams>();
                cfg.CreateMap<RegisterRequest, RegisterParams>();
                cfg.CreateMap<ConfirmRegistrationEmailRequest, ConfirmRegistrationEmailParams>();
                cfg.CreateMap<ForgotPasswordRequest, ForgotPasswordParams>();
                cfg.CreateMap<ResendCodeRequest, ResendCodeParams>();
                cfg.CreateMap<ConfirmPasswordResettingEmailRequest, ConfirmPasswordResettingEmailParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var Params = _mapper.Map<LoginParams>(request);

            var LoginResult = await _authService.Login(Params);

            return LoginResult.IsSuccess ? Ok(new
            {
                user = LoginResult.Value.Item1,
                tokenPair = LoginResult.Value.Item2
            }) : BadRequest(LoginResult.Errors);
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

            return ConfirmEmailResult.IsSuccess ? Ok(new
            {
                user = ConfirmEmailResult.Value.Item1,
                tokenPair = ConfirmEmailResult.Value.Item2
            }) : BadRequest(ConfirmEmailResult.Errors);
        }

        [HttpPost(HttpRoute.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var Params = _mapper.Map<ForgotPasswordParams>(request);
            var ForgotPasswordResult = await _authService.ForgotPassword(Params);
            return ForgotPasswordResult.IsSuccess ? Ok(ForgotPasswordResult.Value) : BadRequest(ForgotPasswordResult.Errors);
        }

        [HttpPost(HttpRoute.ResendCode)]
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request)
        {
            var Params = _mapper.Map<ResendCodeParams>(request);
            var ResendCodeResult = await _authService.ResendCode(Params);
            return ResendCodeResult.IsSuccess ? Ok(ResendCodeResult.Value) : BadRequest(ResendCodeResult.Errors);
        }

        [HttpPost(HttpRoute.ConfirmPasswordResettingEmail)]
        public async Task<IActionResult> ConfirmPasswordResettingEmail([FromBody] ConfirmPasswordResettingEmailRequest request)
        {
            var Params = _mapper.Map<ConfirmPasswordResettingEmailParams>(request);
            var ConfirmPasswordResettingEmailResult = await _authService.ConfirmPasswordResettingEmail(Params);

            return ConfirmPasswordResettingEmailResult.IsSuccess ? Ok(new
            {
                user = ConfirmPasswordResettingEmailResult.Value.Item1,
                tokenPair = ConfirmPasswordResettingEmailResult.Value.Item2
            }) : BadRequest(ConfirmPasswordResettingEmailResult.Errors);
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
