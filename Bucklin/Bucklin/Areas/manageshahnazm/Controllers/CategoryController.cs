using Bucklin.DAL.Context;
using Bucklin.Models;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bucklin.Areas.manageshahnazm.Controllers
{
    [Area("ManageShahnazm")]
    public class CategoryController : Controller
    {
        readonly AppDBContext _context;

        public CategoryController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(bc=>bc.BlogCategories).ThenInclude(b=>b.Blog).
                Include(sc=>sc.StoryCategories).ThenInclude(s=>s.Story).ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> CreateCategory()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategory categoryvm) 
        {
            if(!ModelState.IsValid) return View(categoryvm);
            Category category = new Category
            {
                Name = categoryvm.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateCategory(Guid? id)
        {
            if (id == null) return BadRequest();    
            var category = await _context.Categories.FindAsync(id);
            if(category == null) return NotFound();
            UpdateCategoryVM categoryVM = new UpdateCategoryVM
            {
                Name = category.Name
            };
            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryVM categoryVM, Guid id)
        {
            if (id == null) return BadRequest();
            var category = await _context.Categories.FindAsync(id);
            if(category is null) return NotFound(); 
            category.Name = categoryVM.Name;
            category.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCategory(Guid? id)
        {
            if (id is null) return BadRequest();
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();
            category.IsDeleted = true;
            category.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteComplete(Guid? id)
        {
            if (id is null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletedCategories()
        {
            var categories = await _context.Categories.Where(c=>c.IsDeleted == true).Include(bc => bc.BlogCategories).ThenInclude(b => b.Blog).
                Include(sc => sc.StoryCategories).ThenInclude(s => s.Story).ToListAsync();
            return View(categories);
        }
        
        public async Task<IActionResult> ChangeDeleted(Guid? id)
        {
            if (id is null) return BadRequest();
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();
            category.IsDeleted = false;
            category.UpdatedDate = DateTime.Now;    
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
