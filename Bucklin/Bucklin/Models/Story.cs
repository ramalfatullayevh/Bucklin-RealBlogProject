using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class Story:BaseDatable
	{
        public string Coverİmage { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public bool IsPopular { get; set; }
        public ICollection<StoryCategory>? StoryCategories { get; set; }
        public ICollection<StoryComment> StoryComments { get; set; }


    }
}
