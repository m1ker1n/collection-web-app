using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class TagController : Controller
    {
        private AppDbContext db;

        public TagController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Search(string term)
        {
            var result = term == null ? db.Tags : db.Tags.Where(t => t.Name.StartsWith(term));
            return Json(result.ToList());
        }

        [HttpGet]
        public IActionResult GetItemTags(int id)
        {
            var result = db.TagItems.Where(ti => ti.ItemId == id).Select(ti => new { ti.Tag.Name, ti.TagId });
            return Json(result.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> PostItemTags([FromBody] TagsModel model)
        {
            var item = await db.Items.FindAsync(model.ItemId);
            if (!User.HasAccessToItem(item, db)) return Json(false);
            await DeleteTagItems(item!);
            foreach(var tag in model.Tags)
            {
                var dbTag = await FindTag(tag);
                if (dbTag == null)
                {
                    dbTag = new Tag() { Name = tag.Name };
                    await db.Tags.AddAsync(dbTag);
                    await db.SaveChangesAsync();
                }
                await db.TagItems.AddAsync(new TagItem() { Tag = dbTag, Item = item! });
                await db.SaveChangesAsync();
            }
            return Json(true);
        }

        private async Task<Tag?> FindTag(TagModel model)
        {
            return await db.Tags.FindAsync(model.Id) ?? db.Tags.FirstOrDefault(t => t.Name == model.Name);
        }

        private async Task DeleteTagItems(Item item)
        {
            var tagItems = db.TagItems.Where(ti => ti.Item == item);
            db.TagItems.RemoveRange(tagItems);
            await db.SaveChangesAsync();
        }
    }

    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TagsModel
    {
        public TagModel[] Tags { get; set; }
        
        public int ItemId { get; set; }
    }
}
