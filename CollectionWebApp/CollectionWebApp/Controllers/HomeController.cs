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

        private const int NumOfObjectsToShow = 10;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            var model = CreateHomeIndexModel();
            return View(model);
        }

        private HomeIndexModel CreateHomeIndexModel()
        {
            var items = GetLastCreatedItems(NumOfObjectsToShow);
            var collections = GetMostNumerousCollections(NumOfObjectsToShow);
            HomeIndexModel model = new HomeIndexModel()
            {
                Items = items.ToList(),
                Collections = collections.ToList()
            };
            return model;
        }

        private IQueryable<Item> GetLastCreatedItems(int count)
        {
            var orderedItems = db.Items.OrderByDescending(i => i.Created);
            var items = (orderedItems.Count() > count) ? orderedItems.SkipLast(orderedItems.Count() - count) : orderedItems;
            return items;
        }

        private IQueryable<UserCollection> GetMostNumerousCollections(int count)
        {
            var orderedCollections = db.UserCollections.OrderByDescending(c => c.Items.Count);
            var items = (orderedCollections.Count() > count) ? orderedCollections.SkipLast(orderedCollections.Count() - count) : orderedCollections;
            return items;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}