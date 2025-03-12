using Grpc.Core;
using VaccinationService.DbContexts;
using VaccinationService.Entities;
using Microsoft.EntityFrameworkCore;
using Vaccinations;

namespace VaccinationService.Services
{
	public class VaccinationServiceImpl : Vaccinations.VaccinationService.VaccinationServiceBase
	{
		private readonly VaccinationDbContext _dbContext;

		public VaccinationServiceImpl(VaccinationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public override async Task<VaccineResponse> GetVaccineById(VaccineRequest request, ServerCallContext context)
		{
			var vaccine = await _dbContext.Vaccines.FindAsync(request.VaccineId);
			if (vaccine == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

			return new VaccineResponse
			{
				Id = vaccine.Id,
				Name = vaccine.Name,
				Description = vaccine.Description,
				RecommendedAgeRange = vaccine.RecommendedAgeRange,
				Contraindications = vaccine.Contraindications,
				Price = (double)vaccine.Price
			};
		}

		public override async Task<PackageResponse> GetPackageById(PackageRequest request, ServerCallContext context)
		{
			var package = await _dbContext.Packages
				.Include(p => p.PackageVaccines)
				.ThenInclude(pv => pv.Vaccine)
				.FirstOrDefaultAsync(p => p.Id == request.PackageId);

			if (package == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Package not found"));

			var response = new PackageResponse
			{
				Id = package.Id,
				Name = package.Name,
				Description = package.Description,
				Price = (double)package.Price
			};

			response.Vaccines.AddRange(package.PackageVaccines.Select(pv => new VaccineResponse
			{
				Id = pv.Vaccine.Id,
				Name = pv.Vaccine.Name,
				Description = pv.Vaccine.Description,
				Price = (double)pv.Vaccine.Price
			}));

			return response;
		}
	}
}
