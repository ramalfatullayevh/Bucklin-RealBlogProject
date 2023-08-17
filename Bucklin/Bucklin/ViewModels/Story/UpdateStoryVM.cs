namespace Bucklin.ViewModels
{
    public class UpdateStoryVM
    {
        public string? Coverİmage { get; set; }
        public string Name { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public string Content { get; set; }
        public bool IsPopular { get; set; }

        public IFormFile Cover { get; set; }
    }
}
