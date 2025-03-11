using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using UserManagementService.DbContexts;

namespace UserManagementService.DbContexts
{
	public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
	{
		public UserDbContext CreateDbContext(string[] args)
		{
			// Load cấu hình từ appsettings.json
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
			var connectionString = config.GetConnectionString("DefaultConnection");

			optionsBuilder.UseMySQL(connectionString);

			return new UserDbContext(optionsBuilder.Options);
		}
	}
}
