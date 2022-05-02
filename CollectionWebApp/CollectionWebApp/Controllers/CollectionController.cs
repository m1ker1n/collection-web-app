using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class CollectionController : Controller
    {
        private AppDbContext db;

        public CollectionController(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index(int? id)
        {
            UserCollection? collection = await db.UserCollections.FindAsync(id);
            var model = CreateIndexModel(collection, User.HasAccessTo(collection?.User, db));
            return model == null ? NotFound() : View(model);
        }

        private CollectionIndexModel? CreateIndexModel(UserCollection? collection, bool changeAllowed = false)
        {
            if (collection == null) return null;
            CollectionIndexModel model = new()
            {
                Collection = collection,
                ChangeAllowed = changeAllowed
            };
            return model;
        }
    }
}
