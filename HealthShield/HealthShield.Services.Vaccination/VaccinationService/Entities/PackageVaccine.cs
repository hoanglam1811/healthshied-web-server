namespace VaccinationService.Entities
{
	public class PackageVaccine : BaseEntity
	{
		public int PackageId { get; set; }
		public VaccinationPackage Package { get; set; }

		public int VaccineId { get; set; }
		public Vaccine Vaccine { get; set; }
	}
}
