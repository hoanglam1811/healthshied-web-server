namespace VaccinationService.Entities
{
	public class VaccinationRecord:BaseEntity
	{
		public int Id { get; set; }
		public int AppointmentId { get; set; }
		public int AdministeredBy { get; set; }
		public string ReactionNotes { get; set; }
	}
}
