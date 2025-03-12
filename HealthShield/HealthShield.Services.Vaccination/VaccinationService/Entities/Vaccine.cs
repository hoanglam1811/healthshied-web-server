namespace VaccinationService.Entities
{
	public class Vaccine : BaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; } 
		public string Description { get; set; }
		public string RecommendedAgeRange { get; set; }
		public string Contraindications { get; set; }
		public decimal Price { get; set; }

		public List<PackageVaccine> PackageVaccines { get; set; } = new();
	}
}
