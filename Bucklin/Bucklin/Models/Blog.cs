using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class Blog:BaseDatable
	{
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public bool IsPopular { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<BlogCategory>? BlogCategories { get; set; }
        public ICollection<BlogComment> BlogComments { get; set; }

    }
}
