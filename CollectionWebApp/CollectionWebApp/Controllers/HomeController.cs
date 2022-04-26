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

        private const int ItemsToShow = 10;
        private const int CollectionsToShow = 10;
        private const int TagsToShow = 25;

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
            var items = GetLastCreatedItems(ItemsToShow);
            var collections = GetMostNumerousCollections(CollectionsToShow);
            var tags = GetMostUsableTags(TagsToShow);
            HomeIndexModel model = new HomeIndexModel()
            {
                Items = items.ToList(),
                Collections = collections.ToList(),
                Tags = tags.ToList()
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

        private IQueryable<Tag> GetMostUsableTags(int count)
        {
            var orderedTags = db.Tags.OrderByDescending(t => t.Items.Count);
            var tags = (orderedTags.Count() > count) ? orderedTags.SkipLast(orderedTags.Count() - count) : orderedTags;
            return tags;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}