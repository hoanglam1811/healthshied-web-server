using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Users;
using System.Threading.Tasks;

namespace HealthShield.Controllers
{
	[Route("api/staff-schedule")]
	[ApiController]
	public class StaffScheduleController : ControllerBase
	{
		private readonly UserService.UserServiceClient _grpcClient;

		public StaffScheduleController(UserService.UserServiceClient grpcClient)
		{
			_grpcClient = grpcClient;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetStaffScheduleById(int id)
		{
			try
			{
				var request = new StaffScheduleRequest { StaffScheduleId = id };
				var response = await _grpcClient.GetStaffScheduleByIdAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateStaffSchedule([FromBody] CreateStaffScheduleRequest request)
		{
			try
			{
				var response = await _grpcClient.CreateStaffScheduleAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllStaffSchedules()
		{
			try
			{
				var request = new Empty();
				var response = await _grpcClient.GetAllStaffSchedulesAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateStaffSchedule(int id, [FromBody] UpdateStaffScheduleRequest request)
		{
			try
			{
				request.Id = id;
				var response = await _grpcClient.UpdateStaffScheduleAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStaffSchedule(int id)
		{
			try
			{
				var request = new StaffScheduleRequest { StaffScheduleId = id };
				await _grpcClient.DeleteStaffScheduleAsync(request);
				return NoContent();
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}

		[HttpGet("staff/{staffId}")]
		public async Task<IActionResult> GetScheduleByStaffId(int staffId)
		{
			try
			{
				var request = new StaffRequest { StaffId = staffId };
				var response = await _grpcClient.GetScheduleByStaffIdAsync(request);
				return Ok(response);
			}
			catch (RpcException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Status.Detail);
			}
		}
	}
}
