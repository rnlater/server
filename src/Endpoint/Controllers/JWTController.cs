using Application.Interfaces;
using Endpoint.ApiRequests.JWT;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController(IJWTService jWTService) : ControllerBase
    {
        private readonly IJWTService _jWTService = jWTService;

        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequest request)
        {
            var Result = await _jWTService.RenewAccessToken(request.RefreshToken);
            return Result.IsSuccess ? Ok(Result.Value) : BadRequest(Result.Errors);
        }

    }
}
