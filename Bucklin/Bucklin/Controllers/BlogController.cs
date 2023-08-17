using Bucklin.DAL.Context;
using Bucklin.Models;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bucklin.Controllers
{
	public class BlogController : Controller
	{
        readonly AppDBContext _context;
        readonly UserManager<AppUser> _userManager;


        public BlogController(AppDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
		{
            var blogs = await _context.Blogs.Include(b => b.BlogCategories).ThenInclude(c => c.Category).Include(b => b.BlogComments).ToListAsync();
			return View(blogs);
		}

        public async Task<IActionResult> BlogDetail(Guid id)
        {
            BlogVM story = new BlogVM
            {
                Blog = await _context.Blogs.Include(s => s.BlogComments).ThenInclude(s => s.AppUser).Include(s => s.BlogCategories).ThenInclude(c => c.Category).FirstOrDefaultAsync(s => s.Id == id),
                Blogs = await _context.Blogs.Include(sc => sc.BlogCategories).ThenInclude(c => c.Category).ToListAsync(),


            };
            story.Blog.ViewCount++;
            await _context.SaveChangesAsync();
            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(CreateBlogReviewVM review)
        {
            string userid = _userManager.GetUserId(HttpContext.User);
            if (!ModelState.IsValid) return View(review);
            BlogComment courseReview = new BlogComment
            {
                AppUserId = userid,
                BlogId = review.BlogId,
                ReviewContent = review.ReviewContent,
                ReviewDate = DateTime.Now,
                Status = false,
            };
            if (User.Identity.IsAuthenticated)
            {
                await _context.BlogComments.AddAsync(courseReview);
                await _context.SaveChangesAsync();
                var formattedUrl = Url.Action("blogdetail", "blog", new { id = review.BlogId });
                return Redirect(formattedUrl);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
    }
}
