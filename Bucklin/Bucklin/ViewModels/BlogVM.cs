using Bucklin.Models;

namespace Bucklin.ViewModels
{
	public class BlogVM
	{
        public ICollection<Blog> Blogs { get; set; }
        public Blog Blog { get; set; }
    }
}
