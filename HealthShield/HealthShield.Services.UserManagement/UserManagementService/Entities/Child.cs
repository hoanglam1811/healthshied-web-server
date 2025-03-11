﻿namespace UserManagementService.Entities
{
	public class Child
	{
		public int Id { get; set; }
		public int UserId { get; set; }  // FK tới User
		public string FullName { get; set; }
		public DateTime Birthday { get; set; }
		public string Gender { get; set; }

		public User User { get; set; }  // Navigation property
	}
}
