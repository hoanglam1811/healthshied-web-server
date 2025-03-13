using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Vaccinations;

[Route("api/vaccination")]
[ApiController]
public class VaccinationController : ControllerBase
{
    private readonly VaccinationService.VaccinationServiceClient _grpcClient;

    public VaccinationController(VaccinationService.VaccinationServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    // [HttpGet("vaccine")]
    // public async Task<IActionResult> GetVaccines()
    // {
    //     try
    //     {
    //         var response = await _grpcClient.GetVaccinationRecordByIdAsync(request);
    //         return Ok(response);
    //     }
    //     catch (RpcException ex)
    //     {
    //         return StatusCode((int)ex.StatusCode, ex.Status.Detail);
    //     }
    // }

    [HttpGet("vaccine/{id}")]
    public async Task<IActionResult> GetVaccineById(int id)
    {
        try
        {
            var request = new VaccineRequest { VaccineId = id };
            var response = await _grpcClient.GetVaccineByIdAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPost("vaccine")]
    public async Task<IActionResult> CreateVaccine([FromBody] CreateVaccineRequest request)
    {
        try
        {
            var response = await _grpcClient.CreateVaccineAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }
}
