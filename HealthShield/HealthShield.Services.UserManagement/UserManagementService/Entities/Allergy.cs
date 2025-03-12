namespace UserManagementService.Entities
{
	public class Allergy : BaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

    public List<ChildAllergy> ChildAllergies { get; set; }
	}
}
