using Bucklin.Models;

namespace Bucklin.ViewModels
{
	public class CreateBlogReviewVM
	{
        public string ReviewContent { get; set; }

        public Guid BlogId { get; set; }

        public Blog? Blog { get; set; }
    }
}
