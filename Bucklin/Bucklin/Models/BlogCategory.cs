using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class BlogCategory:BaseId
	{
		public Guid BlogId { get; set; }
		public Guid CategoryId { get; set; }

		public Blog? Blog { get; set; }
		public Category? Category { get; set; }
	}
}
