using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VaccinationService.DbContexts
{
	public class VaccinationDbContextFactory : IDesignTimeDbContextFactory<VaccinationDbContext>
	{
		public VaccinationDbContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<VaccinationDbContext>();
			var connectionString = config.GetConnectionString("DefaultConnection");

			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

			return new VaccinationDbContext(optionsBuilder.Options);
		}
	}
}
