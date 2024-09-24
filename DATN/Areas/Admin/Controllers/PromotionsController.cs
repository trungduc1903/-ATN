using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
using PagedList;
using PagedList.Mvc;
namespace ShopDGHouse.Areas.Admin.Controllers
{
    public class PromotionsController : Controller
    {
        // GET: Admin/Promotions
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Promotion> items = db.Promotions.OrderBy(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.ProName.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Promotion model)
        {
            if (ModelState.IsValid)
            {
                if(model.ToDate<model.FromDate)
                {
                    ViewBag.ErPr = "Dữ liệu nhập không hợp lệ, vui lòng kiểm tra lại";
                }
                else
                {
                    db.Promotions.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }    
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", model.ProductId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands", model.BrandId);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Promotions.Find(id);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", item.ProductId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands", item.BrandId);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Promotion model)
        {
            if (ModelState.IsValid)
            {
                db.Promotions.Attach(model);
                if (model.ToDate < model.FromDate)
                {
                    ViewBag.ErPr = "Dữ liệu sửa không hợp lệ, vui lòng kiểm tra lại";
                }
                else
                {
                    db.Entry(model).Property(x => x.FromDate).IsModified = true;
                    db.Entry(model).Property(x => x.ToDate).IsModified = true;
                    db.Entry(model).Property(x => x.BrandId).IsModified = true;
                    db.Entry(model).Property(x => x.ProductId).IsModified = true;
                    db.Entry(model).Property(x => x.ApplyForAll).IsModified = true;
                    db.Entry(model).Property(x => x.DiscountAmount).IsModified = true;
                    //db.Entry(model).Property(x => x.ProductImages).IsModified = true;
                    db.Entry(model).Property(x => x.DiscountPercent).IsModified = true;
                    db.Entry(model).Property(x => x.Active).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }    
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", model.ProductId);
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "NameBrands", model.BrandId);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Promotions.Find(id);
            if (item != null)
            {
                db.Promotions.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}