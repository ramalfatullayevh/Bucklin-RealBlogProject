using Bucklin.DAL.Context;
using Bucklin.Models;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bucklin.Controllers
{
	public class StoryController : Controller
	{
		readonly AppDBContext _context;
        readonly UserManager<AppUser> _userManager;


        public StoryController(AppDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
		{
            var stories = await _context.Stories.Include(s => s.StoryComments).Include(s => s.StoryCategories).ThenInclude(s => s.Category).ToListAsync();
			return View(stories);
		}

        public async Task<IActionResult> StoryDetail(Guid id)
        {
            StoryVM story = new StoryVM
			{
				Story = await _context.Stories.Include(s => s.StoryComments).Include(s => s.StoryCategories).ThenInclude(c => c.Category).Include(s=>s.StoryComments).ThenInclude(s=>s.AppUser).FirstOrDefaultAsync(s => s.Id == id),
                Stories = await _context.Stories.Include(sc => sc.StoryCategories).ThenInclude(c => c.Category).ToListAsync(),
                
                
            };
            story.Story.ViewCount++;
            await _context.SaveChangesAsync();
            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(CreateStoryReviewVM review)
        {
            string userid = _userManager.GetUserId(HttpContext.User);
            if (!ModelState.IsValid) return View(review);
            StoryComment courseReview = new StoryComment
            {
                AppUserId = userid,
                StoryId = review.StoryId,
                ReviewContent = review.ReviewContent,
                ReviewDate = DateTime.Now,
                Status = false,
            };
            if (User.Identity.IsAuthenticated)
            {
                await _context.StoryComments.AddAsync(courseReview);
                await _context.SaveChangesAsync();
                var formattedUrl = Url.Action("StoryDetail", "Story", new {id = review.StoryId });
                return Redirect(formattedUrl);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
    }
}
