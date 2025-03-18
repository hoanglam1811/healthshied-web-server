using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Users;
using System.Threading.Tasks;
using Authentication;
using HealthShield.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace HealthShield.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService.AuthenticationServiceClient _grpcClient;
		private readonly JwtService _jwtService;
		private readonly IConfiguration _config;

		public AuthenticationController(AuthenticationService.AuthenticationServiceClient grpcClient,
      JwtService jwtService, IConfiguration config)
		{
			_grpcClient = grpcClient;
      _jwtService = jwtService;
      _config = config;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			try
			{
				var response = await _grpcClient.LoginAsync(request);
        var token = _jwtService.CreateToken(_config, response, response.Role);
				return Ok(token);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

    [HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			try
			{
				var response = await _grpcClient.RegisterAsync(request);
        var token = _jwtService.CreateToken(_config, response, response.Role);
				return Ok(token);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

    [HttpGet("test-admin")]
    [Authorize(Roles = "Admin")]
		public string Test()
		{
			return "Hello";
		}

    [HttpGet("test-client")]
    [Authorize(Roles = "Admin,Client")]
		public string TestClient()
		{
			return "Hello";
		}
	}
}
