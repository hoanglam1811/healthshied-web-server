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

    [HttpGet("vaccine")]
    public async Task<IActionResult> GetVaccines()
    {
        try
        {
            var request = new Empty();
            var response = await _grpcClient.GetAllVaccinesAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

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

    [HttpPut("vaccine/{id}")]
    public async Task<IActionResult> UpdateVaccine(int id, [FromBody] CreateVaccineRequest request)
    {
        try
        {
          var requests = new UpdateVaccineRequest
          {
              Id = id,
              Name = request.Name,
              Description = request.Description,
              RecommendedAgeRange = request.RecommendedAgeRange,
              Contraindications = request.Contraindications,
              Price = request.Price
          };
          var response = await _grpcClient.UpdateVaccineAsync(requests);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpDelete("vaccine/{id}")]
    public async Task<IActionResult> DeleteVaccine(int id)
    {
        try
        {
          var request = new VaccineRequest
          {
              VaccineId = id,
          };
          var response = await _grpcClient.DeleteVaccineAsync(request);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }
}
