using Microsoft.EntityFrameworkCore;
using VaccinationService.Entities;

namespace VaccinationService.DbContexts
{
	public class VaccinationDbContext : DbContext
	{
		public DbSet<Vaccine> Vaccines { get; set; }
		public DbSet<VaccinationPackage> Packages { get; set; }
		public DbSet<PackageVaccine> PackageVaccines { get; set; }
		public DbSet<VaccinationRecord> VaccinationRecords { get; set; }

		public VaccinationDbContext(DbContextOptions<VaccinationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PackageVaccine>()
				.HasKey(pv => new { pv.PackageId, pv.VaccineId });

			modelBuilder.Entity<PackageVaccine>()
				.HasOne(pv => pv.Package)
				.WithMany(p => p.PackageVaccines)
				.HasForeignKey(pv => pv.PackageId);

			modelBuilder.Entity<PackageVaccine>()
				.HasOne(pv => pv.Vaccine)
				.WithMany(v => v.PackageVaccines)
				.HasForeignKey(pv => pv.VaccineId);
		}
	}
}
