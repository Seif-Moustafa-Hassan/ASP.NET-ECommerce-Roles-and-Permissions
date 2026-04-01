using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceTest.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // FK → Product
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // FK → User (Identity)
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int Quantity { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}