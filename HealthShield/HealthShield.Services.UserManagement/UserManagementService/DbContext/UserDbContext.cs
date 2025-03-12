using Microsoft.EntityFrameworkCore;
using UserManagementService.Entities;


namespace UserManagementService.DbContexts
{
	public class UserDbContext : DbContext
	{
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Child> Children { get; set; }
		public DbSet<Allergy> Allergies { get; set; }
		public DbSet<ChildAllergy> ChildAllergies { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<StaffSchedule> StaffSchedules { get; set; }

		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(u => u.Children)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);
      modelBuilder.Entity<ChildAllergy>()
        .HasOne(ca => ca.Allergy)
        .WithMany(a => a.ChildAllergies)
        .HasForeignKey(ca => ca.AllergyId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Appointment>()
        .HasOne(a => a.AssignedStaff) 
        .WithMany(u => u.Appointments) 
        .HasForeignKey(a => a.AssignedStaffId) 
        .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
