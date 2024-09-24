using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
using PagedList;
using PagedList.Mvc;

namespace ShopDGHouse.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Brands
        DoAn db = new DoAn();
        public ActionResult Index(int?page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            var items = db.Brands.ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult Detail(int id)
        {
            var item = db.Brands.Find(id);
            return View(item);
        }
    }
}