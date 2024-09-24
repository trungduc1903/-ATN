using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;

namespace ShopDGHouse.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddComments(string Title, string Content,int productId)
        {
            Comment cmt = new Comment();
            if (Session["idUser"] != null)
            {
                cmt.Title = Title;
                cmt.Content = Content;
                cmt.ProductId = productId;
                cmt.CreateDate = DateTime.Now;
                cmt.UserId = (int)Session["idUser"];
                if (ModelState.IsValid)
                {
                    db.Comments.Add(cmt);
                    db.SaveChanges();
                    return RedirectToAction("Detail", "Products", new { id = productId });
                }
            }
            //else
            //{
            //    //ViewBag.error = "Bạn phải đăng nhập để bình luận";
            //    return RedirectToAction("Login", "Account");
            //}
            //return RedirectToAction("Detail", "Products", new { id = productId });
            else
            {
                ViewBag.error = "Bạn phải đăng nhập để bình luận";
            }    
            return RedirectToAction("Detail", "Products", new { id = productId });
        }
    }
}