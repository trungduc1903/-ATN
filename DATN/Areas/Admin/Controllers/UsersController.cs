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
    public class UsersController : Controller
    {
        // GET: Admin/Users
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<User> items = db.Users.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Note.ToString().Contains(Searchtext));
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
        public ActionResult Add(User model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = db.Users.Find(id);
            return View(item);
   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(model);
                db.Entry(model).Property(x => x.Username).IsModified = true;
                db.Entry(model).Property(x => x.Fullname).IsModified = true;
                db.Entry(model).Property(x => x.Password).IsModified = true;
                db.Entry(model).Property(x => x.Email).IsModified = true;
                db.Entry(model).Property(x => x.Note).IsModified = true;
                db.Entry(model).Property(x => x.Active).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Users.Find(id);
            if (item != null && item.Note.Trim() != "Admin")
            {
                db.Users.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            //else
            //{
            //    ViewBag.Errr = "Bạn không thể xoá tài khoản này!";
            //}
            return Json(new { success = false });
        }
    }
}