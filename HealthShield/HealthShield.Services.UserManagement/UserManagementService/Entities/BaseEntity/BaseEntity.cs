namespace UserManagementService.Entities
{
	public class BaseEntity
	{
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string Status { get; set; }
	}
}
