using Microsoft.AspNetCore.Mvc;

using CollectionWebApp.Models;
using CollectionWebApp.ViewModels;

namespace CollectionWebApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private AppDbContext db;

        public AuthorizationController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            //if authorized return to Account page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //if authorized return to account page
            User? user = ValidateLoginModel(model);
            if (!ModelState.IsValid) return View(model);
            //login 
            return RedirectToAction("Index", "Account");
        }

        private User? ValidateLoginModel(LoginModel model)
        {
            User? user = db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) ModelState.AddModelError("", "No such user.");
            if (user != null && user.Password != model.Password) ModelState.AddModelError("", "Wrong password.");
            return user;
        }
    }
}
