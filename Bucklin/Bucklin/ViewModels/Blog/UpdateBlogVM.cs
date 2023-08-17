namespace Bucklin.ViewModels
{
    public class UpdateBlogVM
    {
        public string Name { get; set; }
        public string? Cover { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public string Content { get; set; }
        public bool IsPopular { get; set; }

        public IFormFile CoverImage { get; set; }
    }
}
