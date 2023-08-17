using Bucklin.Models;

namespace Bucklin.ViewModels
{
	public class StoryVM
	{
        public ICollection<Story> Stories { get; set; }
        public Story Story { get; set; }

    }
}
