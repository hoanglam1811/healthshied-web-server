namespace UserManagementService.Entities
{
	public class Feedback : BaseEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int AppointmentId { get; set; }
		public string Rating { get; set; }
		public string Comment { get; set; }

    public Appointment Appointment { get; set; }
    public User User { get; set; }
	}

}
