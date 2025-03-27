using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Vaccinations;

[Route("api/vaccination-package")]
[ApiController]
public class VaccinationPackageController : ControllerBase
{
    private readonly VaccinationService.VaccinationServiceClient _grpcClient;

    public VaccinationPackageController(VaccinationService.VaccinationServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPackages()
    {
        try
        {
            var request = new Empty();
            var response = await _grpcClient.GetAllPackagesAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackageById(int id)
    {
        try
        {
            var request = new PackageRequest { PackageId = id };
            var response = await _grpcClient.GetPackageByIdAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePackage([FromBody] CreatePackageBody request)
    {
        try
        {
            request.Request.VaccineIds.AddRange(request.VaccineIds);
            var response = await _grpcClient.CreatePackageAsync(request.Request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(int id, [FromBody] UpdatePackageBody request)
    {
        try
        {
          request.Request.Id = id;
          request.Request.VaccineIds.AddRange(request.VaccineIds);
          var response = await _grpcClient.UpdatePackageAsync(request.Request);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(int id)
    {
        try
        {
          var request = new PackageRequest
          {
              PackageId = id,
          };
          var response = await _grpcClient.DeletePackageAsync(request);
          return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }
}

public class CreatePackageBody 
{
  public CreatePackageRequest Request { get; set; }
  public List<int> VaccineIds { get; set; }
}

public class UpdatePackageBody 
{
  public UpdatePackageRequest Request { get; set; }
  public List<int> VaccineIds { get; set; }
}
