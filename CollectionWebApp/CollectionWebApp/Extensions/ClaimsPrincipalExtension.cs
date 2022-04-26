using System.Security.Claims;

using CollectionWebApp.Models;

namespace CollectionWebApp.Extensions
{
    public static class PrincipalExtension
    {
        public static User? GetAppUser(this ClaimsPrincipal claimsPrincipal, AppDbContext db)
        {
            if (claimsPrincipal == null || db == null) return null;
            var userEmail = claimsPrincipal.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            User? user = db.Users.FirstOrDefault(u => u.Email == userEmail);
            return user;
        }

        public static bool HasAccessTo(this ClaimsPrincipal claimsPrincipal, User? appUser, AppDbContext db)
        {
            if (appUser == null || claimsPrincipal == null || db == null) return false;
            var appUserOfPrincipal = claimsPrincipal.GetAppUser(db);
            if (appUserOfPrincipal == null) return false;
            if (appUserOfPrincipal.RoleId == db.AdminRoleId || appUser.Id == appUserOfPrincipal.Id) return true;
            return false;
        }

        public static bool HasAccessTo(this ClaimsPrincipal claimsPrincipal, int appUserId, AppDbContext db)
        {
            User? appUser = db.Users.Find(appUserId);
            return claimsPrincipal.HasAccessTo(appUser, db);
        }
    }
}
