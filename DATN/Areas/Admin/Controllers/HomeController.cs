using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopDGHouse.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ShopDGHouse.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        DoAn db = new DoAn();
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmin");
            }
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.Username.Equals(Username) && u.Password.Equals(Password)).ToList();
                if (user.Count() > 0)
                {
                    if (user.Any(u => u.Note.Trim() == "Admin"))
                    {
                        Session["Username"] = user.FirstOrDefault().Username;
                        Session["Fullname"] = user.FirstOrDefault().Fullname;
                        Session["Email"] = user.FirstOrDefault().Email;
                        Session["UserID"] = user.FirstOrDefault().Id;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Bạn không có quyền truy cập, vui lòng đăng nhập lại";
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
            return RedirectToAction("LoginAdmin");
        }
        public ActionResult WirteExcel()
        {
            //var wb = new HSSFWorkbook();
            XSSFWorkbook wb = new XSSFWorkbook();
            var listOrder = db.Orders.ToList();
            var listOrderDetail = db.OrderDetails.ToList();
            //tạo sheet 1
            ISheet sheet1 = wb.CreateSheet("Đơn hàng");
            //tạo sheet2
            ISheet sheet2 = wb.CreateSheet("Đơn hàng chưa xác nhận");
            //ISheet sheet3 = wb.CreateSheet("Đơn hàng chưa xác nhận");
            //ghi đè lên sheet 1
            var row0 = sheet1.CreateRow(0);
            var rowfirst = sheet2.CreateRow(0);
            // Merge lại row đầu 3 cột
            row0.CreateCell(0); // tạo ra cell trc khi merge
            rowfirst.CreateCell(0);
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 7);
            sheet1.AddMergedRegion(cellMerge);
            sheet2.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("Báo cáo");
            rowfirst.GetCell(0).SetCellValue("Báo cáo");

            // Ghi tên cột ở row 1
            var row1 = sheet1.CreateRow(1);
            row1.CreateCell(0).SetCellValue("Ngày tạo đơn hàng");
            row1.CreateCell(1).SetCellValue("Người nhận");
            row1.CreateCell(2).SetCellValue("địa chỉ");
            row1.CreateCell(3).SetCellValue("Email");
            row1.CreateCell(4).SetCellValue("Số điện thoại");
            row1.CreateCell(5).SetCellValue("Phương thức thanh toán");
            //row1.CreateCell(6).SetCellValue("Tên tài khoản");
            row1.CreateCell(7).SetCellValue("Xác nhận");

            //ghi dè sheet2
            var row2 = sheet2.CreateRow(1);
            row2.CreateCell(0).SetCellValue("Ngày tạo đơn hàng");
            row2.CreateCell(1).SetCellValue("Người nhận");
            row2.CreateCell(2).SetCellValue("địa chỉ");
            row2.CreateCell(3).SetCellValue("Email");
            row2.CreateCell(4).SetCellValue("Số điện thoại");
            row2.CreateCell(5).SetCellValue("Phương thức thanh toán");
            //row2.CreateCell(6).SetCellValue("Tên tài khoản");
            row2.CreateCell(7).SetCellValue("Xác nhận");

            // bắt đầu duyệt mảng và ghi tiếp tục
            int rowIndex = 2;
            foreach (var item in listOrder)
            {
                var newRow = sheet1.CreateRow(rowIndex);
                var newRow1 = sheet2.CreateRow(rowIndex);
                // tao row mới                             
                // set giá trị
                if (item.Status == true)
                {
                    newRow.CreateCell(0).SetCellValue(item.OrderDate.ToString());
                    newRow.CreateCell(1).SetCellValue(item.CustomerName);
                    newRow.CreateCell(2).SetCellValue(item.Address);
                    newRow.CreateCell(3).SetCellValue(item.Email);
                    newRow.CreateCell(4).SetCellValue(item.Phone);
                    newRow.CreateCell(5).SetCellValue(item.Payment.PayName);
                    //newRow.CreateCell(6).SetCellValue(item.User.Username.ToString());                   
                    newRow.CreateCell(7).SetCellValue("Yes");
                    rowIndex++;
                }
                else
                {
                    newRow1.CreateCell(0).SetCellValue(item.OrderDate.ToString());
                    newRow1.CreateCell(1).SetCellValue(item.CustomerName);
                    newRow1.CreateCell(2).SetCellValue(item.Address);
                    newRow1.CreateCell(3).SetCellValue(item.Email);
                    newRow1.CreateCell(4).SetCellValue(item.Phone);
                    newRow1.CreateCell(5).SetCellValue(item.Payment.PayName);
                    //newRow1.CreateCell(6).SetCellValue(item.UserId.ToString());
                    newRow1.CreateCell(7).SetCellValue("No");
                    rowIndex++;
                }

            }

            // save file
            string fPath = Path.GetFileName("BaoCao.xlsx");
            string UploadFile = Server.MapPath("~/wwwroot/") + fPath;
            //string test = Server.MapPath("/wwwroot") + "\\" + fPath;
            using (FileStream fs = new FileStream(UploadFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                wb.Write(fs);
            }
            return View("Index");
        }
    }
}