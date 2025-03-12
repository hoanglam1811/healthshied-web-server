using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PaymentService.DbContexts
{
	public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
	{
		public PaymentDbContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
			var connectionString = config.GetConnectionString("DefaultConnection");

			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

			return new PaymentDbContext(optionsBuilder.Options);
		}
	}
}
