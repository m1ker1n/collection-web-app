using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;

namespace CollectionWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private AppDbContext db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            var items = (db.Items.Count() > 10) ? db.Items.OrderByDescending(i => i.Created).SkipLast(db.Items.Count() - 10) : db.Items;
            HomeIndexModel model = new HomeIndexModel()
            {
                Items = items.ToList()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}