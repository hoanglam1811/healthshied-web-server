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

    public override async Task<VaccinesResponse> GetAllVaccines(Empty empty, ServerCallContext context)
		{
			var vaccines = await _dbContext.Vaccines.ToListAsync();

			return new VaccinesResponse
      {
        Vaccines = { vaccines.Select(v => new VaccineResponse
        {
            Id = v.Id,
            Name = v.Name,
            Description = v.Description,
            RecommendedAgeRange = v.RecommendedAgeRange,
            Contraindications = v.Contraindications,
            Price = (double)v.Price
        }) }
      };
		}

    public override async Task<VaccineResponse> CreateVaccine(CreateVaccineRequest request, ServerCallContext context)
		{
			var vaccine = await _dbContext.Vaccines.AddAsync(new Vaccine {
        Name = request.Name,
        Description = request.Description,
        RecommendedAgeRange = request.RecommendedAgeRange,
        Contraindications = request.Contraindications,
        Price = (decimal)request.Price,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        Status = "Active"
      });

      await _dbContext.SaveChangesAsync();
      var vaccineEntity = vaccine.Entity;

			return new VaccineResponse
			{
				Id = vaccineEntity.Id,
				Name = vaccineEntity.Name,
				Description = vaccineEntity.Description,
				RecommendedAgeRange = vaccineEntity.RecommendedAgeRange,
				Contraindications = vaccineEntity.Contraindications,
				Price = (double)vaccineEntity.Price,
			};
		}

    public override async Task<VaccineResponse> UpdateVaccine(UpdateVaccineRequest request, ServerCallContext context)
		{
      var vaccine = await _dbContext.Vaccines.FindAsync(request.Id);
      if (vaccine == null)
          throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

      // Update the existing vaccine entity
      vaccine.Name = request.Name;
      vaccine.Description = request.Description;
      vaccine.RecommendedAgeRange = request.RecommendedAgeRange;
      vaccine.Contraindications = request.Contraindications;
      vaccine.Price = (decimal)request.Price;
      vaccine.UpdatedAt = DateTime.Now;

      _dbContext.Vaccines.Update(vaccine);
      await _dbContext.SaveChangesAsync();

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

    public override async Task<VaccineResponse> DeleteVaccine(VaccineRequest request, ServerCallContext context)
    {
        var vaccine = await _dbContext.Vaccines.FindAsync(request.VaccineId);
        if (vaccine == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Vaccine not found"));

        _dbContext.Vaccines.Remove(vaccine);
        await _dbContext.SaveChangesAsync();

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
