using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class Category:BaseDatable
	{
		public ICollection<BlogCategory>? BlogCategories { get; set; }
		public ICollection<StoryCategory>? StoryCategories { get; set; }
	}
}
