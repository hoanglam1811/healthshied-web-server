using Grpc.Core;
using VaccinationService.Entities;
using Microsoft.EntityFrameworkCore;
using Vaccinations;

namespace VaccinationService.Services
{
	public partial class VaccinationServiceImpl : Vaccinations.VaccinationService.VaccinationServiceBase
	{
    public override async Task<PackageResponse> GetPackageById(PackageRequest request, ServerCallContext context)
		{
			var packages = await _packageRepository.GetAllAsync(q => q
				.Include(p => p.PackageVaccines)
				.ThenInclude(pv => pv.Vaccine)
				);
      var package = packages.FirstOrDefault(p => p.Id == request.PackageId);

			if (package == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Package not found"));

			var response = _mapper.Map<PackageResponse>(package);

			return response;
		}

    public override async Task<PackagesResponse> GetAllPackages(Empty request, ServerCallContext context)
		{
			var packages = await _packageRepository.GetAllAsync(q => q
        .Include(p => p.PackageVaccines)
        .ThenInclude(pv => pv.Vaccine)
      );

			var result = new PackagesResponse();
      result.Packages.AddRange(_mapper.Map<IEnumerable<PackageResponse>>(packages));
      return result;
    }
    public override async Task<PackageResponse> CreatePackage(CreatePackageRequest request, ServerCallContext context)
    {
      var package = await _packageRepository.AddAsync(_mapper.Map<VaccinationPackage>(request));
      foreach (var id in request.VaccineIds)
      {
        await _packageVaccineRepository.AddAsync(new PackageVaccine
        {
          PackageId = package.Id,
          VaccineId = id,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          Status = "Active"
        });
      }
      return _mapper.Map<PackageResponse>(package);
    }

    public override async Task<PackageResponse> UpdatePackage(UpdatePackageRequest request, ServerCallContext context)
    {
      var packages = await _packageRepository.GetAllAsync(q => q
        .Include(p => p.PackageVaccines)
        .ThenInclude(pv => pv.Vaccine)
      );

      var package = packages.FirstOrDefault(p => p.Id == request.Id);
      if (package == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Package not found"));

      var result = await _packageRepository.UpdateAsync(_mapper.Map<VaccinationPackage>(request));
      var mapped = _mapper.Map<PackageResponse>(result);
      if(request.VaccineIds.Count == 0) return _mapper.Map<PackageResponse>(result);

      var packageVaccines = await _packageVaccineRepository.GetAllAsync();
      packageVaccines = packageVaccines.Where(pv => pv.PackageId == package.Id).ToList();

      foreach (var pv in packageVaccines)
      {
        await _packageVaccineRepository.DeleteAsync(pv);
      }

      foreach (var id in request.VaccineIds)
      {
        await _packageVaccineRepository.AddAsync(new PackageVaccine
        {
          PackageId = package.Id,
          VaccineId = id,
          CreatedAt = DateTime.Now,
          UpdatedAt = DateTime.Now,
          Status = "Active"
        });
      }

      packages = await _packageRepository.GetAllAsync(q => q
        .Include(p => p.PackageVaccines)
        .ThenInclude(pv => pv.Vaccine)
      );

      package = packages.FirstOrDefault(p => p.Id == request.Id);


      return _mapper.Map<PackageResponse>(package);
    }

    public override async Task<PackageResponse> DeletePackage(PackageRequest request, ServerCallContext context)
    {
      var result = await _packageRepository.GetByIdAsync(request.PackageId);
      if (result == null)
        throw new RpcException(new Status(StatusCode.NotFound, "Package not found"));

      await _packageRepository.DeleteAsync(result);

      return _mapper.Map<PackageResponse>(result);
    }
  }
}


