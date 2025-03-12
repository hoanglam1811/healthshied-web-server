namespace UserManagementService.Entities
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string PasswordHash { get; set; } 
		public string Role { get; set; }

		public List<Child> Children { get; set; } = new();
		public List<StaffSchedule> StaffSchedules { get; set; } = new();
	}

}
