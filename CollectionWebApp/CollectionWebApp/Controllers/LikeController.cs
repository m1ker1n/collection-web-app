using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class LikeController : Controller
    {
        private AppDbContext db;

        public LikeController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetLikeAmount(int id)
        {
            var result = db.Likes.Where(l => l.ItemId == id).Count();
            return Json(result);
        }

        [HttpPost]
        public IActionResult IsLiked([FromBody] LikeModel model)
        {
            if (model.UserId == 0) return Json(false); //user is not authenticated
            var result = db.Likes.Where(l => l.UserId == model.UserId && l.ItemId == model.ItemId).Count() == 1;
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeModel model)
        {
            var user = await db.Users.FindAsync(model.UserId);
            var item = await db.Items.FindAsync(model.ItemId);
            if (!User.HasAccessTo(user, db) || item == null) return Json(false);
            var like = await db.Likes.FindAsync(model.UserId, model.ItemId);
            if (like == null)
            {
                await db.Likes.AddAsync(new Like() { User = user!, Item = item });
            }
            else
            {
                db.Likes.Remove(like);
            }
            await db.SaveChangesAsync();
            return Json(true);
        }
    }

    public class LikeModel
    {
        public int UserId { get; set; }

        public int ItemId { get; set; }
    }
}
