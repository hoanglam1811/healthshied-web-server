using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UserManagementService.Entities;


namespace UserManagementService.DbContexts
{
	public class UserDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Child> Children { get; set; }

		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(u => u.Children)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);
		}
	}
}
