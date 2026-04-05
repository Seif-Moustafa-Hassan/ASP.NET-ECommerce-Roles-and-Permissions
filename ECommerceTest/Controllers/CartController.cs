using ECommerceTest.Models;
using ECommerceTest.Filters; // Important
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerceTest.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // VIEW CART (User only)
        [PermissionAuthorize("ViewCart", DenyAction = "Forbidden")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var cartItems = db.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            return View(cartItems);
        }

        // ADD TO CART (GET)
        [PermissionAuthorize("AddToCart", DenyAction = "Forbidden")]
        public ActionResult Add(int? productId)
        {
            if (productId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            return View(product);
        }

        // ADD TO CART (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("AddToCart", DenyAction = "Forbidden")]
        public ActionResult Add(int productId, int quantity)
        {
            var userId = User.Identity.GetUserId();
            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            var existingItem = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.UpdatedAt = DateTime.Now;
            }
            else
            {
                db.Carts.Add(new Cart
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            db.SaveChanges();

            TempData["SuccessMessage"] = "Product added to cart successfully!";
            return RedirectToAction("Index", "Product");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}