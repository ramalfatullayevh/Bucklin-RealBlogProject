using Bucklin.DAL.Context;
using Bucklin.Helpers;
using Bucklin.Models;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Bucklin.Areas.manageshahnazm.Controllers
{
    [Area("manageshahnazm")]
    public class BlogsController : Controller
    {
        readonly AppDBContext _context;
        readonly IWebHostEnvironment _env;

        public BlogsController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(bc => bc.BlogCategories).ThenInclude(c => c.Category).ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> CreateBlog()
        {
            ViewBag.Category = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreateBlogVM blogVM)
        {
            var coverImg = blogVM.CoverImage;
            if (coverImg != null)
            {
                string result = coverImg.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Category = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(blogVM);
            } 
            
            Blog blog = new Blog
            {
                Name = blogVM.Name,
                Content = blogVM.Content,
                CreatedDate = DateTime.Now,
                IsPopular = blogVM.IsPopular,
                ViewCount = 0,
                ImageUrl = blogVM.CoverImage.SaveFile(Path.Combine(_env.WebRootPath, "user", "assets", "blogimg")),

            };

           

            var categories = _context.Categories.Where(ctg => blogVM.CategoryIds.Contains(ctg.Id));

            foreach (var item in categories)
            {
                _context.BlogCategories.Add(new BlogCategory { Blog = blog, CategoryId = item.Id });
            }

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBlog(Guid? id)
        {
            if (id is null) return BadRequest();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog is null) return NotFound();
            blog.IsDeleted = true;
            blog.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteComplete(Guid? id)
        {
            if (id is null) return NotFound();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog is null) return NotFound();
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletedBlogs()
        {
            var blogs = await _context.Blogs.Where(c => c.IsDeleted == true).Include(sc => sc.BlogCategories).ThenInclude(s => s.Category).ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> ChangeDeleted(Guid? id)
        {
            if (id is null) return BadRequest();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog is null) return NotFound();
            blog.IsDeleted = false;
            blog.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateBlog(Guid? id)
        {
            if (id == null) return BadRequest();
            var blog = _context.Blogs.Include(sc => sc.BlogCategories).FirstOrDefault(c => c.Id == id);
            if (blog == null) return NotFound();
            ViewBag.Category = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            UpdateBlogVM storyVM = new UpdateBlogVM
            {
                Name = blog.Name,
                Content = blog.Content,
                IsPopular = blog.IsPopular,
                Cover = blog.ImageUrl,
                CategoryIds = new List<Guid>(),

            };
            foreach (var category in blog.BlogCategories)
            {
                storyVM.CategoryIds.Add(category.CategoryId);
            }
            return View(storyVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(UpdateBlogVM storyVM, Guid id)
        {
            if (id == null) return BadRequest();
            var blog = _context.Blogs.Include(sc => sc.BlogCategories).FirstOrDefault(c => c.Id == id);
            if (blog is null) return NotFound();
            blog.Name = storyVM.Name;
            blog.Content = storyVM.Content;
            blog.IsPopular = storyVM.IsPopular;
            blog.UpdatedDate = DateTime.Now;
            foreach (var category in blog.BlogCategories)
            {
                if (storyVM.CategoryIds.Contains(category.CategoryId))
                {
                    storyVM.CategoryIds.Remove(category.CategoryId);
                }
                else
                {
                    _context.BlogCategories.Remove(category);
                }
            }
            foreach (var categoryId in storyVM.CategoryIds)
            {
                _context.BlogCategories.Add(new BlogCategory { Blog = blog, CategoryId = categoryId });
            }

            if (storyVM.CoverImage != null)
            {
                string result = storyVM.CoverImage.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }

                blog.ImageUrl.DeleteFile(_env.WebRootPath, "user/assets/blogimg");
                blog.ImageUrl = storyVM.CoverImage.SaveFile(Path.Combine(_env.WebRootPath, "user", "assets", "blogimg"));
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
