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
    public class OrdersController : Controller
    {
        // GET: Admin/Orders
        DoAn db = new DoAn();
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.OrderDate.ToString().Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Details(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }
        public ActionResult Add()
        {
            ViewBag.PaymentId = new SelectList(db.Payments, "Id", "PayName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Order model)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(model);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = model.Id });
            }
            ViewBag.PaymentId = new SelectList(db.Payments, "Id", "PayName",model.PaymentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username",model.UserId);
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = db.Orders.Find(id);
            ViewBag.PaymentId = new SelectList(db.Payments, "Id", "PayName", item.PaymentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", item.UserId);
            ViewBag.productId = new SelectList(db.Products, "Id", "ProductName");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Attach(model);
                db.Entry(model).Property(x => x.OrderDate).IsModified = true;
                db.Entry(model).Property(x => x.UserId).IsModified = true;
                db.Entry(model).Property(x => x.PaymentId).IsModified = true;
                db.Entry(model).Property(x => x.Address).IsModified = true;
                db.Entry(model).Property(x => x.CustomerName).IsModified = true;
                db.Entry(model).Property(x => x.Email).IsModified = true;
                db.Entry(model).Property(x => x.Phone).IsModified = true;
                db.Entry(model).Property(x => x.Status).IsModified = true;
                if (model.Status == true)
                {
                    foreach (var item in db.OrderDetails)
                    {
                        foreach (var pr in db.Products)
                        {
                            if (item.OrderId == model.Id)
                            {
                                if (pr.Id == item.ProductId)
                                {
                                    if(pr.Quantity < item.Quantity)
                                    {
                                        ViewBag.Loi = "Sản phẩm không đủ số lượng.Vui lòng cập nhật lại số lượng sản phẩm trong kho!!!";
                                        //return RedirectToAction("Edit", new { id = model.Id });
                                        ViewBag.PaymentId = new SelectList(db.Payments, "Id", "PayName", model.PaymentId);
                                        ViewBag.UserId = new SelectList(db.Users, "Id", "Username", model.UserId);
                                        ViewBag.productId = new SelectList(db.Products, "Id", "ProductName");
                                        return View(model);
                                    }
                                    else
                                    {

                                        pr.Quantity -= item.Quantity;                               
                                    }    
                                }
                            }
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentId = new SelectList(db.Payments, "Id", "PayName", model.PaymentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", model.UserId);
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOD(int orderId, int productId, int quantity)
        {
            try
            {
                var takePro = db.Products.Where(s => s.Id == productId).ToList();
                var checkkm = from km in db.Promotions where (km.Active == true && km.FromDate <= DateTime.Now && km.ToDate >= DateTime.Now) select km;
                var result = from obj in takePro
                             from km in checkkm.ToList()
                             where obj.Id == km.ProductId || obj.BrandId == km.BrandId || km.ApplyForAll == true
                             select obj;
                var kmapplyforall = db.Promotions.Where(km => km.ApplyForAll == true).ToList();
                Boolean checkQuyen(int code)
                {
                    Boolean check = false;
                    foreach (var abc in result.ToList())
                    {
                        if (abc.Id == code)
                        {
                            check = true;
                        }
                    }
                    return check;
                }
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = orderId;
                orderDetail.ProductId = productId;
                orderDetail.Quantity = quantity;

                foreach (var item in takePro)
                {
                    if (checkQuyen(productId) == false)
                    {
                        orderDetail.Price = (decimal)item.Price;
                    }
                    if (kmapplyforall.Count > 0)
                    {
                        foreach (var km in checkkm)
                        {
                            if (km.ApplyForAll == true)
                            {
                                if (km.DiscountPercent != null)
                                {
                                    orderDetail.Price = (decimal)((item.Price - (decimal)(item.Price * (decimal)km.DiscountPercent / 100)) * quantity);
                                }
                                else if (km.DiscountAmount != null)
                                {
                                    orderDetail.Price = (decimal)((item.Price - (decimal)km.DiscountAmount) * quantity);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var km in checkkm)
                        {
                            if (km.ProductId == productId || km.BrandId == item.BrandId)
                            {
                                if (km.DiscountPercent != null)
                                {
                                    orderDetail.Price = (decimal)((item.Price - (decimal)(item.Price * (decimal)km.DiscountPercent / 100)) * quantity);
                                }
                                else if (km.DiscountAmount != null)
                                {
                                    orderDetail.Price = (decimal)((item.Price - (decimal)km.DiscountAmount) * quantity);
                                }
                            }
                        }
                    }
                }
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = orderId });
            }
            catch (Exception ex)
            {
                ViewBag.error = "Không thành công" + ex.ToString();
                return RedirectToAction("Edit", new { id = orderId });
            }
        }
        [HttpPost]
        public ActionResult EditOD(int quantity, int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (quantity > 0)
            {
                orderDetail.Price = (orderDetail.Price / orderDetail.Quantity) * quantity;
                orderDetail.Quantity = quantity;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = orderDetail.OrderId });
            }
            else
            {
                return RedirectToAction("DeleteOD", new { id = id, orderId = orderDetail.OrderId });
            }
        }
        public ActionResult DeleteOD(int id, int orderId)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = orderId });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                foreach (var obj in db.OrderDetails)
                {
                    if (obj.OrderId == id)
                    {
                        db.OrderDetails.Remove(obj);
                        db.Orders.Remove(item);
                    }
                }
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}