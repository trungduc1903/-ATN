using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
namespace ShopDGHouse.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu\
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var item = db.Categories.ToList();
            return PartialView(item);
        }
        public ActionResult MenuLeft()
        {
            var item = db.Categories.ToList();
            return PartialView(item);
        }
        public ActionResult MenuDown()
        {
            var item = db.Categories.ToList();
            return PartialView(item);
        }
    }
}