using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceTest.Models
{
    public class RolePermission
    {
        [Key, Column(Order = 0)]
        public string RoleId { get; set; }  // FK to AspNetRoles.Id

        [Key, Column(Order = 1)]
        public int PermissionId { get; set; } // FK to Permissions.Id

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}