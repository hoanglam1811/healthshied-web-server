using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PaymentService.DbContexts
{
	public class PaymentDbContext : DbContext
	{
		public DbSet<Payment> Payments { get; set; }

		public DbSet<Notification> Notifications { get; set; }

		public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Payment>()
				.Property(p => p.Amount)
				.HasColumnType("decimal(18,2)");
		}
	}
}
