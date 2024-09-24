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
    public class ProductsController : Controller
    {
        // GET: Products
        DoAn db = new DoAn();
        public ActionResult Index(int? id, int?page,string sortOrder,string thuonghieu,string keyword)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            var items = db.Products.ToList();
            //IEnumerable<Product> items = db.Products.OrderBy(x => x.Id);
            if (id != null)
            {
                items = items.Where(x => x.Id == id).ToList();
            }
            //var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //items = items.ToPagedList(pageIndex, pageSize);
            //SortOrder
            List<string> listsort = new List<string> { "Sắp theo tên Z-A", "Giá tăng dần", "Giá giảm dần", "Khuyến mãi" };
            ViewBag.sortOrder = new SelectList(listsort);
            ViewBag.thuonghieu = new SelectList(db.Brands, "NameBrands", "NameBrands");
            ViewBag.THSearch = thuonghieu;
            ViewBag.CurrentSort = sortOrder;
            if (!string.IsNullOrEmpty(keyword))
            {
                page = 1;
                items = items.Where(obj =>
                                       obj.ProductName.ToUpper().Contains(keyword.ToUpper())
                                       ).Select(s => s).ToList();

            }
            if (!string.IsNullOrEmpty(thuonghieu))
            {
                items = items.Where(obj =>
                                       obj.Brand.NameBrands.Equals(thuonghieu)
                                       ).Select(s => s).ToList();
            }
            switch (sortOrder)
            {
                case "Sắp theo tên Z-A":
                    items = items.OrderByDescending(s => s.ProductName).ToList();
                    break;

                case "Giá tăng dần":
                    items = items.OrderBy(s => s.Price).ToList();
                    break;

                case "Giá giảm dần":
                    items = items.OrderByDescending(s => s.Price).ToList();
                    break;
                case "Khuyến mãi":
                    var checkkm = from km in db.Promotions where (km.Active == true && km.FromDate <= DateTime.Now && km.ToDate >= DateTime.Now) select km;
                    var result = from obj in items
                                 from km in checkkm.ToList()
                                 where obj.Id == km.ProductId || obj.BrandId == km.BrandId || km.ApplyForAll == true
                                 orderby obj.ProductName
                                 select obj;
                    items = result.ToList();
                    break;
                default:
                    items = items.OrderBy(s => s.ProductName).ToList();
                    break;
            }
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult GetSanPham(int id, int? page, string sortOrder, string thuonghieu)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.CategoryId == id).ToList();
            }
            List<string> listsort = new List<string> { "Sắp theo tên Z-A", "Giá tăng dần", "Giá giảm dần", "Khuyến mãi" };
            ViewBag.sortOrder = new SelectList(listsort);
            ViewBag.thuonghieu = new SelectList(db.Brands, "NameBrands", "NameBrands");
            ViewBag.THSearch = thuonghieu;
            ViewBag.CurrentSort = sortOrder;
            if (!string.IsNullOrEmpty(thuonghieu))
            {
                items = items.Where(obj =>
                                       obj.Brand.NameBrands.Equals(thuonghieu)
                                       ).Select(s => s).ToList();
            }
            switch (sortOrder)
            {
                case "Sắp theo tên Z-A":
                    items = items.OrderByDescending(s => s.ProductName).ToList();
                    break;

                case "Giá tăng dần":
                    items = items.OrderBy(s => s.Price).ToList();
                    break;

                case "Giá giảm dần":
                    items = items.OrderByDescending(s => s.Price).ToList();
                    break;
                case "Khuyến mãi":
                    var checkkm = from km in db.Promotions where (km.Active == true && km.FromDate <= DateTime.Now && km.ToDate >= DateTime.Now) select km;
                    var result = from obj in items
                                 from km in checkkm.ToList()
                                 where obj.Id == km.ProductId || obj.BrandId == km.BrandId || km.ApplyForAll == true
                                 orderby obj.ProductName
                                 select obj;
                    items = result.ToList();
                    break;
                default:
                    items = items.OrderBy(s => s.ProductName).ToList();
                    break;
            }
            var cate = db.Categories.Find(id);
            ViewBag.cateName = cate.CategoryName;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items.ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult Partial_Menu()
        {
            var items = db.Products.ToList();
            return PartialView(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }
    }
}