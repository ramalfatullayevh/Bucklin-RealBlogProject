using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class StoryCategory:BaseId
	{
        public Guid StoryId { get; set; }
        public Guid CategoryId { get; set; }

        public Story? Story { get; set; }
        public Category? Category { get; set; }
    }
}
