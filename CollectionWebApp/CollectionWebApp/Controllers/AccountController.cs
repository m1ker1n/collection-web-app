using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;
using CollectionWebApp.Extensions;

namespace CollectionWebApp.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db;

        public AccountController(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index(int? id)
        {
            User? user = id == null ? User.GetAppUser(db) : await db.Users.FindAsync(id);
            var model = CreateAccountModel(user, User.HasAccessTo(user, db));
            return model == null ? NotFound() : View(model);
        }

        private AccountModel? CreateAccountModel(User? user, bool changeAllowed = false)
        {
            if (user == null) return null;
            AccountModel model = new()
            { 
                User = user!,
                ChangeAllowed = changeAllowed
            };
            return model;
        }
    }
}
