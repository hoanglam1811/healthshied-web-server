using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Vaccinations;

[Route("api/vaccination-record")]
[ApiController]
public class VaccinationRecordController : ControllerBase
{
    private readonly VaccinationService.VaccinationServiceClient _grpcClient;

    public VaccinationRecordController(VaccinationService.VaccinationServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecords()
    {
        try
        {
            var request = new Empty();
            var response = await _grpcClient.GetAllVaccinationRecordsAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecordById(int id)
    {
        try
        {
            var request = new VaccinationRecordRequest { RecordId = id };
            var response = await _grpcClient.GetVaccinationRecordByIdAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecord([FromBody] CreateVaccinationRecordRequest request)
    {
        try
        {
            var response = await _grpcClient.CreateVaccinationRecordAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] UpdateVaccinationRecordRequest request)
    {
        try
        {
          request.Id = id;
          var response = await _grpcClient.UpdateVaccinationRecordAsync(request);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        try
        {
          var request = new VaccinationRecordRequest
          {
              RecordId = id,
          };
          var response = await _grpcClient.DeleteVaccinationRecordAsync(request);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }
}
