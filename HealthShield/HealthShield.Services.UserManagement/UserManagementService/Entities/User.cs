namespace UserManagementService.Entities
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string PasswordHash { get; set; }  // Mã hóa mật khẩu
		public string Role { get; set; } // (Admin, Staff, Parent)

		public List<Child> Children { get; set; } = new();
	}

}
