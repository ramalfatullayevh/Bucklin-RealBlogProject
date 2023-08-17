using Bucklin.DAL.Context;
using Bucklin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bucklin.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM
            {
                Categories = await _context.Categories.Include(bc=>bc.BlogCategories).ThenInclude(b=>b.Blog).ToListAsync(),
                Stories = await _context.Stories.Include(sc=>sc.StoryCategories).ThenInclude(c=>c.Category).ToListAsync(),
                Blogs = await _context.Blogs.Include(bc => bc.BlogCategories).ThenInclude(c => c.Category).Include(b=>b.BlogComments).ToListAsync(),
            };
            return View(vm);
        }

        
    }
}
