using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Entities
{
	public class ChildAllergy : BaseEntity
	{
		public int Id { get; set; }
		public int ChildId { get; set; }
		public int AllergyId { get; set; }

    public Child Child { get; set; }
    public Allergy Allergy { get; set; }
	}
}
