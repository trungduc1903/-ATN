using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Net;

namespace ShopDGHouse.Areas.Admin.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Admin/Brands
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Brand> items = db.Brands.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.NameBrands.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Brand model)
        {
            if (ModelState.IsValid)
            {
                model.ImageBrands = "";
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = Path.GetFileName(f.FileName);
                    string UpLoadFile = Server.MapPath("~/wwwroot/ImageBrands/") + FileName;
                    f.SaveAs(UpLoadFile);
                    model.ImageBrands = FileName;
                }
                db.Brands.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = db.Brands.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand model)
        {
            if (ModelState.IsValid)
            {
                //model.ImageBrands = "";
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = Path.GetFileName(f.FileName);
                    string UpLoadFile = Server.MapPath("~/wwwroot/ImageBrands/") + FileName;
                    f.SaveAs(UpLoadFile);
                    model.ImageBrands = FileName;
                }
                db.Brands.Attach(model);
                db.Entry(model).Property(x => x.NameBrands).IsModified = true;
                db.Entry(model).Property(x => x.ImageBrands).IsModified = true;
                db.Entry(model).Property(x => x.Active).IsModified = true;
                db.Entry(model).Property(x => x.Describe).IsModified = true;
                db.Entry(model).Property(x => x.ChiNhanh).IsModified = true;
                db.Entry(model).Property(x => x.Address).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Brands.Find(id);
            if (item != null)
            {
                db.Brands.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}