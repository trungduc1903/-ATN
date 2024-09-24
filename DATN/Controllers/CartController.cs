using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
namespace ShopDGHouse.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.ChekCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Checkout()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(CustomerViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                Cart cart = (Cart)Session["Cart"];
                if (cart != null)
                {
                    Order dh = new Order();
                    dh.Phone = req.Phone;
                    dh.PaymentId = req.PayMent;
                    dh.Address = req.Address;
                    dh.OrderDate = DateTime.Now;
                    dh.Status = false;
                    if(Session["idUser"]!=null)
                    {
                        dh.UserId = (int)Session["idUser"];
                        dh.Email = (string)Session["EmailUI"];
                        dh.CustomerName = (string)Session["FullnameUI"];
                    }   
                    else
                    {
                        dh.UserId = null;
                        dh.Email = req.Email;
                        dh.CustomerName = req.CustumerName;

                    }

                    var checkkm = from km in db.Promotions where (km.Active == true && km.FromDate <= DateTime.Now && km.ToDate >= DateTime.Now) select km;
                    var result = from obj in cart.items
                                 from km in checkkm.ToList()
                                 where obj.ProductID == km.ProductId || obj.BrandID == km.BrandId || km.ApplyForAll == true
                                 select obj;
                    var kmapplyforall = db.Promotions.Where(km => km.ApplyForAll == true).ToList();
                    Boolean checkQuyen(int codes)
                    {
                        Boolean check = false;
                        foreach (var abc in result.ToList())
                        {
                            if (abc.ProductID == codes)
                            {
                                check = true;
                            }
                        }
                        return check;
                    }
                    foreach (var item in cart.items)
                    {
                        OrderDetail odt = new OrderDetail();
                        odt.ProductId = item.ProductID;
                        odt.Quantity = item.Quantity;
                        if(checkQuyen(item.ProductID)==false)
                        {
                            odt.Price = item.Price * item.Quantity;
                        }
                        if (kmapplyforall.Count > 0)
                        {
                            foreach (var km in checkkm)
                            {
                                if (km.ApplyForAll == true)
                                {
                                    if (km.DiscountPercent != null)
                                    {
                                        odt.Price = (decimal)((item.Price - (decimal)(item.Price * (decimal)km.DiscountPercent / 100)) * item.Quantity);
                                    }
                                    else if (km.DiscountAmount != null)
                                    {
                                        odt.Price = (decimal)((item.Price - (decimal)km.DiscountAmount) * item.Quantity);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var km in checkkm)
                            {
                                if (km.ProductId == item.ProductID || km.BrandId == item.BrandID)
                                {
                                    if (km.DiscountPercent != null)
                                    {
                                        odt.Price = (decimal)((item.Price - (decimal)(item.Price * (decimal)km.DiscountPercent / 100)) * item.Quantity);
                                    }
                                    else if (km.DiscountAmount != null)
                                    {
                                        odt.Price = (decimal)((item.Price - (decimal)km.DiscountAmount) * item.Quantity);
                                    }
                                }
                            }
                        }      
                        dh.OrderDetails.Add(odt);
                    }
                    db.Orders.Add(dh);
                    db.SaveChanges();
                    code = new { Success = true, Code = 1 };
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var tongtien = decimal.Zero;
                    foreach (var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + sp.Price + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    tongtien = thanhtien;
                    string payname = db.Payments.FirstOrDefault(s => s.Id == req.PayMent).PayName;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", dh.Id.ToString());
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", dh.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", dh.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", dh.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", dh.Address);
                    contentCustomer = contentCustomer.Replace("{{PayMent}}", payname);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}",thanhtien.ToString());
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", tongtien.ToString());
                    ShopDGHouse.Common.Common.SendMail("DCSHOP", "Đơn hàng #" + dh.Id.ToString(), contentCustomer.ToString(), dh.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", dh.Id.ToString());
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", dh.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", dh.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", dh.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", dh.Address);
                    contentAdmin = contentAdmin.Replace("{{PayMent}}", payname);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", thanhtien.ToString());
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", tongtien.ToString());
                    ShopDGHouse.Common.Common.SendMail("DCSHOP", "Đơn hàng #" + dh.Id.ToString(), contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        public ActionResult Partial_Item_Thanhtoan()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {           
                return PartialView(cart.items);
            }            
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new DoAn();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                Cart cart = (Cart)Session["Cart"];
                if (cart == null)
                {
                    cart = new Cart();
                }
                if(quantity>checkProduct.Quantity)
                {
                    code = new { Success = false, msg = "Số lượng hàng lớn hơn số lượng còn trong kho, vui lòng đặt lại", code = 1, Count = cart.items.Count };
                }
                else
                {
                    CartItem item = new CartItem
                    {
                        ProductID = checkProduct.Id,
                        BrandID = checkProduct.Brand.Id,
                        ProductName = checkProduct.ProductName,
                        CategoryName = checkProduct.Category.CategoryName,
                        Quantity = quantity
                    };
                    foreach (var img in checkProduct.ProductImages)
                    {
                        if (img.IsDefault == true)
                        {
                            string ImagePath = "~/wwwroot/ImageProducts/" + img.ImagePath;
                            item.Img = ImagePath;
                        }
                    }
                    //item.Img = checkProduct.ProductImages.FirstOrDefault(s=>s.IsDefault==true);
                    item.Price = (decimal)checkProduct.Price;
                    item.TotalPrice = item.Quantity * item.Price;
                    cart.AddToCart(item, quantity);
                    Session["Cart"] = cart;
                    code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.items.Count };
                }    
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.items.FirstOrDefault(x => x.ProductID == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                cart.Update(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}