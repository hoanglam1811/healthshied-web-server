using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Users;
using System.Threading.Tasks;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly UserService.UserServiceClient _grpcClient;

	public UserController(UserService.UserServiceClient grpcClient)
	{
		_grpcClient = grpcClient;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(int id)
	{
		try
		{
			var request = new UserRequest { UserId = id };
			var response = await _grpcClient.GetUserByIdAsync(request);
			return Ok(response);
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}

	[HttpPost]
	public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
	{
		try
		{
			var response = await _grpcClient.CreateUserAsync(request);
			return Ok(response);
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetAllUsers()
	{
		try
		{
			var request = new Empty();
			var response = await _grpcClient.GetAllUsersAsync(request);
			return Ok(response);
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
	{
		try
		{
			request.Id = id;
			var response = await _grpcClient.UpdateUserAsync(request);
			return Ok(response);
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		try
		{
			var request = new UserRequest { UserId = id };
			await _grpcClient.DeleteUserAsync(request);
			return NoContent();
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}

	[HttpGet("children/{customerId}")]
	public async Task<IActionResult> GetChildByCustomerId(int customerId)
	{
		try
		{
			var request = new UserRequest { UserId = customerId };
			var response = await _grpcClient.GetChildByCustomerIdAsync(request);
			return Ok(response);
		}
		catch (RpcException ex)
		{
			return StatusCode((int)ex.StatusCode, ex.Status.Detail);
		}
	}
}
