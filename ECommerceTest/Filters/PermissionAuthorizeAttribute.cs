using ECommerceTest.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceTest.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _permission;

        /// <summary>
        /// What to do if authorization fails:
        /// "Forbidden" → return 403
        /// "Redirect" → redirect to AccessDenied page
        /// Default is "Redirect"
        /// </summary>
        public string DenyAction { get; set; } = "Redirect";

        public PermissionAuthorizeAttribute(string permission)
        {
            _permission = permission;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var userId = httpContext.User.Identity.GetUserId();

            using (var db = new ApplicationDbContext())
            {
                // Get all roles of the current user
                var userRoles = db.Users
                    .Where(u => u.Id == userId)
                    .SelectMany(u => u.Roles.Select(r => r.RoleId))
                    .ToList();

                // Check if any of the user's roles has the required permission
                var hasPermission = db.RolePermissions
                    .Any(rp => userRoles.Contains(rp.RoleId)
                               && rp.Permission.Name == _permission);

                return hasPermission;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // User is logged in but lacks permission
                if (DenyAction.Equals("Forbidden", StringComparison.OrdinalIgnoreCase))
                {
                    filterContext.Result = new HttpStatusCodeResult(403);
                }
                else
                {
                    // Default → redirect to AccessDenied page
                    filterContext.Result = new RedirectResult("/Account/AccessDenied");
                }
            }
            else
            {
                // User is not logged in → standard redirect to login
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}