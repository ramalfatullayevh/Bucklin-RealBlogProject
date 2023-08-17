using Bucklin.Models;

namespace Bucklin.ViewModels
{
	public class CreateStoryReviewVM
	{
        public string ReviewContent { get; set; }

        public Guid StoryId { get; set; }

        public Story? Story { get; set; }
    }
}
