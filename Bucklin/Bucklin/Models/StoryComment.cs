using Bucklin.Models.BaseEntity;

namespace Bucklin.Models
{
    public class StoryComment:BaseId
    {
        public string ReviewContent { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool Status { get; set; }


        public string AppUserId { get; set; }

        public AppUser? AppUser { get; set; }


        public Guid StoryId { get; set; }

        public Story? Story { get; set; }
    }
}
