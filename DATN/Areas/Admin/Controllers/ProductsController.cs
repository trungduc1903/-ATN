using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace ShopDGHouse.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderBy(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Category.CategoryName.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "Id", "CategoryName");
            ViewBag.BrandId = new SelectList(db.Brands.ToList(), "Id", "NameBrands");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(model);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Products.Find(id);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName",item.CategoryId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands",item.BrandId);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                db.Products.Attach(model);
                db.Entry(model).Property(x => x.ProductName).IsModified = true;
                db.Entry(model).Property(x => x.Price).IsModified = true;
                db.Entry(model).Property(x => x.BrandId).IsModified = true;
                db.Entry(model).Property(x => x.CategoryId).IsModified = true;
                db.Entry(model).Property(x => x.TGPH).IsModified = true;
                db.Entry(model).Property(x => x.Describe).IsModified = true;
                //db.Entry(model).Property(x => x.ProductImages).IsModified = true;
                db.Entry(model).Property(x => x.Origin).IsModified = true;
                db.Entry(model).Property(x => x.Quantity).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName",model.CategoryId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands",model.BrandId);
            return View(model);
        }
        [HttpPost]
        public ActionResult AddImage(int productId, bool isDefault)
        {
            ProductImage productImage = new ProductImage();
            productImage.ProductId = productId;
            productImage.IsDefault = isDefault;
            productImage.CreateDate = DateTime.Now;
            productImage.ImagePath = "";
            var f = Request.Files["ImageFile"];
            if (f != null && f.ContentLength > 0)
            {
                string FileName = Path.GetFileName(f.FileName);
                string UpLoadFile = Server.MapPath("~/wwwroot/ImageProducts/") + FileName;
                f.SaveAs(UpLoadFile);
                productImage.ImagePath = FileName;
            }
            db.ProductImages.Add(productImage);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = productId });
        }
        public ActionResult DeleteImage(int id, int productId)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = productId });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                foreach (var obj in db.ProductImages)
                {
                    if (obj.ProductId == id)
                    {
                        db.ProductImages.Remove(obj);
                        db.Products.Remove(item);
                    }
                }
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}