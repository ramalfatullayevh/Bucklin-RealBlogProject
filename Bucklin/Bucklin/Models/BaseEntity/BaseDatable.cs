namespace Bucklin.Models.BaseEntity
{
	public class BaseDatable:BaseId
	{
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public DateTime DeletedDate { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
