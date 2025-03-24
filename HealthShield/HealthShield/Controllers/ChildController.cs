using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Users;
using System.Threading.Tasks;

namespace HealthShield.Controllers
{
	[Route("api/child")]
	[ApiController]
	public class ChildController : ControllerBase
	{
		private readonly UserService.UserServiceClient _grpcClient;

		public ChildController(UserService.UserServiceClient grpcClient)
		{
			_grpcClient = grpcClient;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetChildById(int id)
		{
			try
			{
				var request = new ChildRequest { ChildId = id };
				var response = await _grpcClient.GetChildByIdAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateChild([FromBody] CreateChildRequest request)
		{
			try
			{
				var response = await _grpcClient.CreateChildAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllChildren()
		{
			try
			{
				var request = new Empty();
				var response = await _grpcClient.GetAllChildrenAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

    [HttpGet("by-customer-id/{id}")]
		public async Task<IActionResult> GetAllChildrenByCustomer(int id)
		{
			try
			{
				var request = new UserRequest{
          UserId = id
        };
				var response = await _grpcClient.GetChildByCustomerIdAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateChild(int id, [FromBody] UpdateChildRequest request)
		{
			try
			{
				request.Id = id;
				var response = await _grpcClient.UpdateChildAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteChild(int id)
		{
			try
			{
				var request = new ChildRequest { ChildId = id };
				await _grpcClient.DeleteChildAsync(request);
				return NoContent();
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}
	}
}
