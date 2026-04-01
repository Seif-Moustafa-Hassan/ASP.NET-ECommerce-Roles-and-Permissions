using ECommerceTest.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ECommerceTest.Helpers
{
    public static class PermissionHelper
    {
        public static bool HasPermission(string permissionName)
        {
            // ✅ Check authentication
            var user = HttpContext.Current?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return false;

            var userId = user.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return false;

            using (var db = new ApplicationDbContext())
            {
                // ✅ Get user role IDs safely
                var roleIds = db.Users
                    .Where(u => u.Id == userId)
                    .SelectMany(u => u.Roles.Select(r => r.RoleId))
                    .ToList();

                if (!roleIds.Any())
                    return false;

                // ✅ Check permission with explicit join (safer than navigation property)
                var hasPermission = db.RolePermissions
                    .Include(rp => rp.Permission)
                    .Any(rp =>
                        roleIds.Contains(rp.RoleId) &&
                        rp.Permission != null &&
                        rp.Permission.Name == permissionName
                    );

                return hasPermission;
            }
        }
    }
}