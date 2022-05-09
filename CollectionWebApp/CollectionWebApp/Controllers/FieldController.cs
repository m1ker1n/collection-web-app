using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class FieldController : Controller
    {
        private AppDbContext db;

        public FieldController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var collection = await db.UserCollections.FindAsync(id);
            if (!User.HasAccessTo(collection?.User, db)) return NotFound();
            var model = CreateCreateModel(collection!.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FieldCreateModel model)
        {
            var collection = await db.UserCollections.FindAsync(model.UserCollectionId);
            if (!ModelState.IsValid || !User.HasAccessTo(collection?.User, db)) return View(model);
            var field = await CreateField(model);
            if (field == null) return NotFound();
            await db.Fields.AddAsync(field);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Collection", new { id = collection!.Id });
        }

        private async Task<Field> CreateField(FieldCreateModel model)
        {
            var collection = await db.UserCollections.FindAsync(model.UserCollectionId);
            if (collection == null) return null;
            var field = new Field()
            {
                Name = model.Name,
                Type = model.Type,
                UserCollection = collection
            };
            return field;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var field = await db.Fields.FindAsync(id);
            if (!User.HasAccessTo(field?.UserCollection?.User, db)) return NotFound();
            var model = CreateEditModel(field!);
            return View(model);
        }

        private FieldCreateModel CreateCreateModel(int collectionId)
        {
            return new() { UserCollectionId = collectionId };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FieldEditModel model)
        {
            var field = await db.Fields.FindAsync(model.Id);
            if (!ModelState.IsValid || !User.HasAccessTo(field?.UserCollection?.User, db)) return View(model);
            await EditField(field, model);
            return RedirectToAction("Index", "Collection", new { id = field!.UserCollectionId});
        }

        private async Task EditField(Field field, FieldEditModel model)
        {
            field.Name = model.Name;
            field.Type = model.Type;
            await db.SaveChangesAsync();
        }

        private FieldEditModel CreateEditModel(Field field)
        {
            return new()
            {
                Name = field.Name,
                Id = field.Id,
                Type = field.Type,
                UserCollectionId = field.UserCollectionId
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var field = await db.Fields.FindAsync(id);
            if (field == null || !User.HasAccessTo(field.UserCollection.User, db)) return NotFound();
            db.Remove(field);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Collection", new { id = field.UserCollectionId });
        }
    }
}
