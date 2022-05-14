using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;
using CollectionWebApp.Extensions;
using CollectionWebApp.Services;

namespace CollectionWebApp.Controllers
{
    public class CollectionController : Controller
    {
        private AppDbContext db;
        private IImageStorage storage;

        public CollectionController(AppDbContext db, IImageStorage storage)
        {
            this.db = db;
            this.storage = storage;
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

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            User? user = await db.Users.FindAsync(id);
            if (!User.HasAccessTo(user, db)) return NotFound();
            var model = CreateCreateModel(user!);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableThemes = GetAvailableThemes();
                return View(model);
            }
            var collection = await CreateCollection(model);
            return RedirectToAction("Edit", "Collection", new {id = collection.Id});
        }

        private CollectionCreateModel CreateCreateModel(User user)
        {
            return new()
            {
                UserId = user!.Id,
                AvailableThemes = db.Themes.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList()
            };
        }
        
        private async Task<UserCollection> CreateCollection(CollectionCreateModel model)
        {
            Theme theme = await db.Themes.FindAsync(model.ThemeId);
            User user = await db.Users.FindAsync(model.UserId);
            UserCollection collection = new()
            {
                Name = model.Name!,
                Description = model.Description,
                Theme = theme,
                ImageUrl = storage.ImagePlaceholderUrl,
                User = user
            };
            await db.UserCollections.AddAsync(collection);
            await db.SaveChangesAsync();
            return collection;
        }

        private List<SelectListItem> GetAvailableThemes()
        {
            return db.Themes.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            UserCollection? collection = await db.UserCollections.FindAsync(id);
            if (!User.HasAccessTo(collection?.User, db)) return NotFound();
            var model = CreateEditModel(collection!);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CollectionEditModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableThemes = GetAvailableThemes();
                model.Fields = (await db.UserCollections.FindAsync(model.CollectionId) as UserCollection)!.Fields;
                return View(model);
            }
            var collection = await EditCollection(model);
            if (collection == null) return NotFound();
            return RedirectToAction("Edit", "Collection", new { id = collection!.Id });
        }

        private CollectionEditModel CreateEditModel(UserCollection collection)
        {
            return new()
            {
                CollectionId = collection.Id,
                Name = collection.Name,
                Description = collection.Description,
                ThemeId = collection.ThemeId,
                ImageUrl = collection.ImageUrl,
                UserId = collection.UserId,
                AvailableThemes = GetAvailableThemes(),
                Fields = collection.Fields
            };
        }

        private async Task<UserCollection?> EditCollection(CollectionEditModel model)
        {
            Theme theme = await db.Themes.FindAsync(model.ThemeId);
            UserCollection collection = await db.UserCollections.FindAsync(model.CollectionId);
            if (model.File != null)
                await UploadFile(model);
            if (collection == null) return null;
            collection.Name = model.Name;
            collection.Description = model.Description;
            collection.Theme = theme;
            collection.ImageUrl = model.ImageUrl;
            await db.SaveChangesAsync();
            return collection;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var collection = await db.UserCollections.FindAsync(id);
            if (collection == null) return NotFound();
            db.UserCollections.Remove(collection);
            return RedirectToAction("Index", "Account", new { id = User.GetAppUser(db)!.Id });
        }

        private async Task UploadFile(CollectionEditModel model)
        {
            if (model.File == null) return;
            var url = await storage.UploadFileAsync(model.File, model.File.FileName);
            model.ImageUrl = url;
        }
    }
}
