using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;

namespace ShopDGHouse.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Username,Password,Fullname,Email,Note,Active")] User us)
        {
            var tkcheck = db.Users.Where(tk => tk.Username == us.Username);
            if (tkcheck.Count() == 0)
            {
                if (ModelState.IsValid)
                {
                    us.Note = "Member";
                    us.Active = true;
                    db.Users.Add(us);
                    db.SaveChanges();

                }
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.error = "Tài khoản đã tồn tại";
                return View(us);
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.Username.Equals(Username) && u.Password.Equals(Password)).ToList();
                if (user.Count() > 0)
                {
                    //add session
                    if(user.FirstOrDefault().Active==true)
                    {
                        Session["FullnameUI"] = user.FirstOrDefault().Fullname;
                        Session["UsernameUI"] = user.FirstOrDefault().Username;
                        Session["EmailUI"] = user.FirstOrDefault().Email;
                        Session["idUser"] = user.FirstOrDefault().Id;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Tài khoản của bạn đã bị khoá! Vui lòng liên hệ với cửa hàng qua hotline: 0332967513 để được hỗ trợ!";
                    }    
                }
                else
                {
                    ViewBag.error = "Tài khoản hoặc mật khẩu sai! Vui lòng đăng nhập lại!!!!";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult ProfileUser(int? id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }
        public ActionResult ChangePass(int?id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult ChangePass(int id,string Password, string passwordnew, string comfirmpass)
        {
            User users = db.Users.Find(id);
            if (Password.Equals(users.Password))
            {
                if (!passwordnew.Equals(users.Password))
                {
                    if (comfirmpass.Equals(passwordnew))
                    {
                        users.Password = passwordnew;
                        db.SaveChanges();
                        Session["idUser"] = null;
                        Session["UsernameUI"] = null;
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.error = "xác nhận lại mật khẩu!";
                        return View(users);
                    }
                }
                else
                {
                    ViewBag.error = "Mật khẩu mới trùng với mật khẩu cũ";
                    return View(users);
                }

            }
            else
            {
                ViewBag.error = "Mật khẩu cũ không đúng";
                return View(users);
            }
        }
    }
}