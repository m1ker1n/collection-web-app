using System.Security.Claims;

using CollectionWebApp.Models;

namespace CollectionWebApp.Extensions
{
    public static class PrincipalExtension
    {
        public static User? GetAppUser(this ClaimsPrincipal claimsPrincipal, AppDbContext db)
        {
            if (claimsPrincipal == null || db == null) return null;
            var userEmail = claimsPrincipal.GetEmail();
            User? user = db.Users.FirstOrDefault(u => u.Email == userEmail);
            return user;
        }

        public static bool HasAccessTo(this ClaimsPrincipal claimsPrincipal, User? appUser, AppDbContext db)
        {
            if (appUser == null || claimsPrincipal == null || db == null) return false;
            var appUserOfPrincipal = claimsPrincipal.GetAppUser(db);
            if (appUserOfPrincipal == null) return false;
            if (appUserOfPrincipal.RoleId == Constants.AdminRoleId || appUser.Id == appUserOfPrincipal.Id) return true;
            return false;
        }

        public static bool HasAccessTo(this ClaimsPrincipal claimsPrincipal, int appUserId, AppDbContext db)
        {
            User? appUser = db.Users.Find(appUserId);
            return claimsPrincipal.HasAccessTo(appUser, db);
        }

        public static bool HasAccessToCollection(this ClaimsPrincipal claimsPrincipal, int collectionId, AppDbContext db)
        {
            UserCollection? collection = db.UserCollections.Find(collectionId);
            return claimsPrincipal.HasAccessToCollection(collection, db);
        }

        public static bool HasAccessToCollection(this ClaimsPrincipal claimsPrincipal, UserCollection? collection, AppDbContext db)
        {
            return claimsPrincipal.HasAccessTo(collection?.User, db);
        }

        public static bool HasAccessToItem(this ClaimsPrincipal claimsPrincipal, int itemId, AppDbContext db)
        {
            Item? item = db.Items.Find(itemId);
            return claimsPrincipal.HasAccessToItem(item, db);
        }

        public static bool HasAccessToItem(this ClaimsPrincipal claimsPrincipal, Item? item, AppDbContext db)
        {
            return claimsPrincipal.HasAccessToCollection(item?.UserCollection, db);
        }

        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole(Constants.AdminRole);
        }

        public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null) return null;
            return claimsPrincipal.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
        }
    }
}
