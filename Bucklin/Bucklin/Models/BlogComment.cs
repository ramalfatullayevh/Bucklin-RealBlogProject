using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
	public class BlogComment:BaseId
	{
        public string ReviewContent { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool Status { get; set; }


        public string AppUserId { get; set; }

        public AppUser? AppUser { get; set; }


        public Guid BlogId { get; set; }

        public Blog? Blog { get; set; }
    }
}
