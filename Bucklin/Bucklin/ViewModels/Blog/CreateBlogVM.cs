namespace Bucklin.ViewModels
{
    public class CreateBlogVM
    {
        public string Name { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public string Content { get; set; }
        public bool IsPopular { get; set; }

        public IFormFile CoverImage { get; set; }
    }
}
