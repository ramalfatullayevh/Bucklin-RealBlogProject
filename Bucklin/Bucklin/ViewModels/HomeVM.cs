using Bucklin.Models;

namespace Bucklin.ViewModels
{
    public class HomeVM
    {
        public ICollection<Category> Categories { get; set; }
        public ICollection<Story> Stories { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}
