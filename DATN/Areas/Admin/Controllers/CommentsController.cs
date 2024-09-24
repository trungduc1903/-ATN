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
    public class CommentsController : Controller
    {
        // GET: Admin/Comments
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Comment> items = db.Comments.OrderBy(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Comments.Find(id);
            if (item != null)
            {
                db.Comments.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}