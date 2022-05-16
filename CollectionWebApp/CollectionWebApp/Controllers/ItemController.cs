using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class ItemController : Controller
    {
        private AppDbContext db;

        public ItemController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            Item? item = await db.Items.FindAsync(id);
            var model = CreateIndexModel(item, User.HasAccessToItem(item, db));
            return model == null ? NotFound() : View(model);
        }

        private ItemIndexModel? CreateIndexModel(Item? item, bool changeAllowed = false)
        {
            if (item == null) return null;
            return new() { Item = item, ChangeAllowed = changeAllowed, User = User.GetAppUser(db) };
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            UserCollection? collection = await db.UserCollections.FindAsync(id);
            if (!User.HasAccessToCollection(collection, db)) return NotFound();
            var model = CreateCreateModel(collection);
            return model == null ? NotFound() : View(model);
        }

        private ItemCreateModel? CreateCreateModel(UserCollection? collection)
        {
            if (collection == null) return null;
            return new()
            {
                UserCollectionId = collection.Id,
                Name = String.Empty
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemCreateModel model)
        {
            if (!User.HasAccessToCollection(model.UserCollectionId, db)) return NotFound();
            if (!ModelState.IsValid) return View(model);
            var item = await CreateItem(model);
            return RedirectToAction("Edit", "Item", new {id = item.Id});
        }

        private async Task<Item> CreateItem(ItemCreateModel model)
        {
            var collection = await db.UserCollections.FindAsync(model.UserCollectionId);
            var item = new Item()
            {
                UserCollection = collection,
                Name = model.Name,
            };
            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();
            return item;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await db.Items.FindAsync(id);
            if (!User.HasAccessToItem(item, db)) return NotFound();
            var model = CreateEditModel(item);
            return model == null ? NotFound() : View(model);
        }

        private ItemEditModel? CreateEditModel(Item? item)
        {
            if (item == null) return null;
            ICollection<Field> fields = item!.UserCollection?.Fields ?? new List<Field>();
            ICollection<FieldItem> fieldItems = new List<FieldItem>();
            int fieldsAmount = fields.Count;
            foreach(var field in fields)
            {
                var fieldItem = field.FieldItems.FirstOrDefault(fi => fi.Item == item);
                if (fieldItem == null)
                {
                    fieldItem = new FieldItem() { Field = field, Item = item };
                    db.FieldItems.Add(fieldItem);
                    db.SaveChanges();
                }
                fieldItems.Add(fieldItem);
            }
            return new()
            {
                ItemId = item.Id,
                UserCollectionId = item.UserCollectionId,
                Name = item.Name,
                FieldItems = fieldItems.Select(fi => CreateFieldItemModel(fi)).ToList<FieldItemModel>()
            };
        }

        private FieldItemModel CreateFieldItemModel(FieldItem fi)
        {
            return new()
            {
                ItemId = fi.ItemId,
                FieldId = fi.FieldId,
                FieldName = fi.Field.Name,
                Type = fi.Field.Type,
                BoolValue = fi.BoolValue,
                DateValue = fi.DateValue,
                NumberValue = fi.NumberValue,
                StringValue = fi.StringValue,
                TextValue = fi.TextValue
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemEditModel model)
        {
            if (!User.HasAccessToItem(model.ItemId, db)) return NotFound();
            if (!ModelState.IsValid) return View(model);
            var item = await db.Items.FindAsync(model.ItemId);
            await EditItem(item, model);
            return RedirectToAction("Edit", "Item", new { id = item?.Id });
        }

        private async Task EditItem(Item? item, ItemEditModel model)
        {
            if (item == null) return;
            item.Name = model.Name;
            foreach(var fi in model.FieldItems)
            {
                await EditFieldItem(fi);
            }
            await db.SaveChangesAsync();
        }
        
        private async Task EditFieldItem(FieldItemModel model)
        {
            var fi = await db.FieldItems.FindAsync(model.FieldId, model.ItemId);
            if (fi == null) return;
            fi.TextValue = model.TextValue;
            fi.BoolValue = model.BoolValue;
            fi.NumberValue = model.NumberValue;
            fi.DateValue = model.DateValue;
            fi.StringValue = model.StringValue;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await db.Items.FindAsync(id);
            if (!User.HasAccessToItem(item, db)) return NotFound();
            db.Items.Remove(item!);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Collection", new { id = item?.UserCollection?.Id });
        }
    }
}