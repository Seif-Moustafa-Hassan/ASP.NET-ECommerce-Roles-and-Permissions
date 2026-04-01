using ECommerceTest.Models;
using ECommerceTest.Filters; // ✅ مهم جداً
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerceTest.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ✅ READ (Admin + User)
        [PermissionAuthorize("Read")]
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        // ✅ READ (Admin + User)
        [PermissionAuthorize("Read")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // ✅ CREATE (Admin only)
        [PermissionAuthorize("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // ✅ CREATE (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Create")]
        public ActionResult Create([Bind(Include = "Name,Description,Price,Quantity,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;

                db.Products.Add(product);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // ✅ UPDATE (Admin only)
        [PermissionAuthorize("Update")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // ✅ UPDATE (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Update")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Quantity,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                var dbProduct = db.Products.Find(product.Id);
                if (dbProduct == null)
                    return HttpNotFound();

                dbProduct.Name = product.Name;
                dbProduct.Description = product.Description;
                dbProduct.Price = product.Price;
                dbProduct.Quantity = product.Quantity;
                dbProduct.Status = product.Status;
                dbProduct.UpdatedAt = DateTime.Now;

                db.SaveChanges();

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // ✅ DELETE (Admin only)
        [PermissionAuthorize("Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // ✅ DELETE (Admin only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            db.Products.Remove(product);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}