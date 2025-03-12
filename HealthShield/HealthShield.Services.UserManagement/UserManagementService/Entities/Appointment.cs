namespace UserManagementService.Entities
{
	public class Appointment : BaseEntity
	{
		public int Id { get; set; }
		public int AssignedStaffId { get; set; }
		public int ChildId { get; set; }
		public int VaccineId { get; set; }
		public int PackageId { get; set; }
		public int RecordId { get; set; }
		public string Status { get; set; }
		public DateTime AppointmentDate { get; set; }

    public User AssignedStaff { get; set; }
    public Child Child { get; set; }
    public List<Feedback> Feedbacks { get; set; } = new();
	}

}
