using ECommerceTest.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceTest.Filters
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _permission;

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
                var userRoles = db.Users
                    .Where(u => u.Id == userId)
                    .SelectMany(u => u.Roles.Select(r => r.RoleId))
                    .ToList();

                var hasPermission = db.RolePermissions
                    .Any(rp => userRoles.Contains(rp.RoleId)
                            && rp.Permission.Name == _permission);

                return hasPermission;
            }
        }
    }
}