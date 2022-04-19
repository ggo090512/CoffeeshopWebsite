using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTH_Coffee.Models;

namespace DTH_Coffee.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLQuanCafeDataContext db = new dbQLQuanCafeDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["NhaplaiMK"];
            var diachi = collection["Diachi"];
            var email = collection["email"];
            var dienthoai = collection["sdt"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["Ngaysinh"]);
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Hãy nhập tên đăng nhập";
            }
            else if (tendn.Length < 6)
            {
                ViewData["Loi7"] = "Tên đăng nhập phải có ít nhất 6 ký tự";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Hãy nhập mật khẩu";
            }
            else if (matkhau.Length < 6)
            {
                ViewData["Loi4"] = "Mật khẩu phải có ít nhất 6 ký tự";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Hãy nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Số điện thoại không được bỏ trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TK = tendn;
                kh.MK = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Hãy nhập tên đăng nhập";
            }
            else if (tendn.Length < 6)
            {
                ViewData["Loi3"] = "Tên đăng nhập phải có ít nhất 6 ký tự";
            }
            //else if ()
            //{
            //    ViewData["Loi5"] = "Tên đăng nhập phải có ít nhất 1 chữ số ";
            //}
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Hãy nhập mật khẩu";
            }
            else if (matkhau.Length < 6)
            {
                ViewData["Loi4"] = "Mật khẩu phải có ít nhất 6 ký tự";
            }
            //else if ()
            //{
            //    ViewData["Loi6"] = "Mật khẩu phải có ít nhất 1 chữ số ";
            //}
            
            else
            {
                //gan gi tri cho doi tuong dc tao moi
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TK == tendn && n.MK == matkhau);
                if (kh != null)
                {
                    Session["TK"] = kh;
                    return RedirectToAction("Index", "DTH_Coffee");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult TTCT()
        {
            return View();
        }
       
    }
}