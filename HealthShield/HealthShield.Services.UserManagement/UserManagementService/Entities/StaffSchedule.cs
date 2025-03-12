namespace UserManagementService.Entities
{
	public class StaffSchedule:BaseEntity
	{
			public int Id { get; set; }
			public int StaffId { get; set; }
			public DateTime ShiftDate { get; set; }
			public ShiftTime ShiftTime { get; set; }
		}

	public enum ShiftTime {
		MORNING,
		AFTERNOON,
		EVENING
	}
}
