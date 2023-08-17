using Bucklin.DAL.Context;
using Bucklin.Helpers;
using Bucklin.Models;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bucklin.Areas.manageshahnazm.Controllers
{
    [Area("manageshahnazm")]
    public class StoriesController : Controller
    {
        readonly AppDBContext _context;
        readonly IWebHostEnvironment _env;

        public StoriesController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories.Include(sc => sc.StoryCategories).ThenInclude(c => c.Category).ToListAsync();
            return View(stories);
        }

        public async Task<IActionResult> CreateStory()
        {
            ViewBag.Category = new SelectList( _context.Categories, nameof(Category.Id), nameof(Category.Name));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(CreateStoryVM storyVM)
        {
            if (storyVM.Cover != null)
            {
                string result = storyVM.Cover.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }
            }
            if (!ModelState.IsValid) return View(storyVM);
            Story story = new Story
            {
                Name = storyVM.Name,
                Content = storyVM.Content,
                CreatedDate = DateTime.Now,
                IsPopular = storyVM.IsPopular,
                ViewCount = 0,
                Coverİmage = storyVM.Cover.SaveFile(Path.Combine(_env.WebRootPath, "user", "assets", "storyimg")),
            };


            var categories = _context.Categories.Where(ctg => storyVM.CategoryIds.Contains(ctg.Id));

            foreach (var item in categories)
            {
                _context.StoryCategories.Add(new StoryCategory { Story = story, CategoryId = item.Id });
            }

            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));   
        }

        public async Task<IActionResult> DeleteStory(Guid? id)
        {
            if (id is null) return BadRequest();
            var story = await _context.Stories.FindAsync(id);
            if (story is null) return NotFound();
            story.IsDeleted = true;
            story.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteComplete(Guid? id)
        {
            if (id is null) return NotFound();
            var story = await _context.Stories.FindAsync(id);
            if (story is null) return NotFound();
             _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletedStories()
        {
            var stories = await _context.Stories.Where(c => c.IsDeleted == true).Include(sc => sc.StoryCategories).ThenInclude(s => s.Category).ToListAsync();
            return View(stories);
        }

        public async Task<IActionResult> ChangeDeleted(Guid? id)
        {
            if (id is null) return BadRequest();
            var story = await _context.Stories.FindAsync(id);
            if (story is null) return NotFound();
            story.IsDeleted = false;
            story.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateStory(Guid? id)
        {
            if (id == null) return BadRequest();
            var story = _context.Stories.Include(sc => sc.StoryCategories).FirstOrDefault(c => c.Id == id);
            if (story == null) return NotFound();
            ViewBag.Category = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            UpdateStoryVM storyVM = new UpdateStoryVM
            {
                Name = story.Name,
                Content = story.Content,
                IsPopular = story.IsPopular,
                Coverİmage = story.Coverİmage,
                CategoryIds = new List<Guid>(),

            };
            foreach (var category in story.StoryCategories)
            {
                storyVM.CategoryIds.Add(category.CategoryId);
            }
            return View(storyVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStory(UpdateStoryVM storyVM, Guid id)
        {
            if (id == null) return BadRequest();
            var story = _context.Stories.Include(sc => sc.StoryCategories).FirstOrDefault(c => c.Id == id);
            if (story is null) return NotFound();
            story.Name = storyVM.Name;
            story.Content = storyVM.Content;
            story.IsPopular = storyVM.IsPopular;
            story.UpdatedDate = DateTime.Now;
            foreach (var category in story.StoryCategories)
            {
                if (storyVM.CategoryIds.Contains(category.CategoryId))
                {
                    storyVM.CategoryIds.Remove(category.CategoryId);
                }
                else
                {
                    _context.StoryCategories.Remove(category);
                }
            }
            foreach (var categoryId in storyVM.CategoryIds)
            {
                _context.StoryCategories.Add(new StoryCategory { Story = story, CategoryId = categoryId });
            }

            if (storyVM.Cover != null)
            {
                string result = storyVM.Cover.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }

                story.Coverİmage.DeleteFile(_env.WebRootPath, "user/assets/storyimg");
                story.Coverİmage = storyVM.Cover.SaveFile(Path.Combine(_env.WebRootPath, "user", "assets", "storyimg"));
            }
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
