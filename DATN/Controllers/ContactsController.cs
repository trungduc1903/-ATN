using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
namespace ShopDGHouse.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id,CtName,Email,Phone,Address,Content")] Contact ct)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(ct);
                db.SaveChanges();
                ViewBag.Success = ct;
            }
            return View(ct);
        }
    }
}