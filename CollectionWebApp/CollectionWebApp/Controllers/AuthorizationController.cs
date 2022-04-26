﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Account");
            User? user = ValidateLoginModel(model);
            if (!ModelState.IsValid) return View(model);
            await LoginUserAsync(user!); 
            return RedirectToAction("Index", "Account");
        }

        private User? ValidateLoginModel(LoginModel model)
        {
            User? user = db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) ModelState.AddModelError("", "No such user.");
            if (user != null && user.Password != model.Password) ModelState.AddModelError("", "Wrong password.");
            return user;
        }

        private async Task LoginUserAsync(User user)
        {
            await AuthenticateAsync(user.Email, user.Role.Name);
        }

        private async Task AuthenticateAsync(string email, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}