namespace VaccinationService.Entities
{
	public class VaccinationPackage : BaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public List<PackageVaccine> PackageVaccines { get; set; } = new();
	}
}
