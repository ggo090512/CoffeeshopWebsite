using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTH_Coffee.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;


namespace DTH_Coffee.Controllers
{
    public class AdminController : Controller
    {
        dbQLQuanCafeDataContext db = new dbQLQuanCafeDataContext();
        // GET: Admin
        public ActionResult Index1()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //gan cac gia tri nguoi dung nhap lieu cho cac bien
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Hãy nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Hãy nhập mật khẩu";
            }
            else if (matkhau.Length < 6)
            {
                ViewData["Loi4"] = "Mật khẩu phải có ít nhất 6 ký tự";
            }
            else
            {
                //gan gia tri cho doi tuong duoc tao moi (ad)
                AD ad = db.ADs.SingleOrDefault(n => n.UserName == tendn && n.PassWord == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index1", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult DoiMK()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoiMK(FormCollection collection)
        {
            var matkhaucu = collection["MatKhauCu"];
            var matkhaumoi = collection["MatKhauMoi"];
            var nhaplaiMK = collection["NhaplaiMK"];
            if (String.IsNullOrEmpty(matkhaucu))
            {
                ViewData["Loi1"] = "Hãy nhập mật khẩu cũ";
            }
            else if (String.IsNullOrEmpty(matkhaumoi))
            {
                ViewData["Loi2"] = "Hãy nhập mật khẩu mới";
            }
            else if (String.IsNullOrEmpty(nhaplaiMK))
            {
                ViewData["Loi3"] = "Hãy nhập lại mật khẩu mới";
            }
            else
            {
                //gan gi tri cho doi tuong dc tao moi
                AD ad = (AD)Session["Taikhoanadmin"];
                AD admin = db.ADs.SingleOrDefault(n =>n.PassWord == matkhaucu);
                if (matkhaucu != ad.PassWord)
                {
                    ViewBag.Thongbao = "Mật khẩu cũ không chính xác";
                }
                else
                {

                    admin.PassWord = matkhaumoi;
                    UpdateModel(admin);
                    db.SubmitChanges();
                    Session["Taikhoanadmin"] = null;
                    return RedirectToAction("Login");
                }
            }
            return this.DoiMK();


        }

        public ActionResult Cafe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.CAFEs.ToList());
            return View(db.CAFEs.ToList().OrderBy(n => n.MaCafe).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Themcaphe()
        {
            //Dua du lieu vao dropdownlist
            //Lay ds tu table loai caphe
            ViewBag.MaLoai = new SelectList(db.LOAICAFEs.ToList().OrderBy(n => n.TenLoaiCafe), "MaLoai", "TenLoaiCafe");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themcaphe(CAFE cafe, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAICAFEs.ToList().OrderBy(n => n.TenLoaiCafe), "MaLoai", "TenLoaiCafe");
            //Kiem tra duong dan
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using System.IO
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Luu duong dan
                    var path = Path.Combine(Server.MapPath("~/image products"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    cafe.Anh = fileName;
                    //luu vao CSDL
                    db.CAFEs.InsertOnSubmit(cafe);
                    db.SubmitChanges();

                }
                return RedirectToAction("Cafe");
            }

        }
        //Hien thi san pham
        public ActionResult Chitiet(int id)
        {
            //Lay doi tuong cafe theo ma
            CAFE cafe = db.CAFEs.SingleOrDefault(n => n.MaCafe == id);
            ViewBag.MaCafe = cafe.MaCafe;
            if (cafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cafe);
        }
        //Xoa cafe
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            //Lay cafe theo ma
            CAFE cafe = db.CAFEs.SingleOrDefault(n => n.MaCafe == id);
            ViewBag.MaCafe = cafe.MaCafe;
            if (cafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cafe);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong can xoa theo ma
            CAFE cafe = db.CAFEs.SingleOrDefault(n => n.MaCafe == id);
            ViewBag.MaCafe = cafe.MaCafe;
            if (cafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CAFEs.DeleteOnSubmit(cafe);
            db.SubmitChanges();
            return RedirectToAction("Cafe");
        }

        //sửa thông tin sản phẩm
        [HttpGet]
        public ActionResult Sua(int id)
        {
            //Lay sản phẩm theo mã
            CAFE cafe = db.CAFEs.SingleOrDefault(n => n.MaCafe == id);
            ViewBag.MaCafe = cafe.MaCafe;
            ViewBag.Mota = cafe.MoTa;
            if (cafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.LOAICAFEs.ToList().OrderBy(n => n.TenLoaiCafe), "MaLoai", "TenLoaiCafe", cafe.MaLoai);
            return View(cafe);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(CAFE cafe, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAICAFEs.ToList().OrderBy(n => n.TenLoaiCafe), "MaLoai", "TenLoaiCafe");
            CAFE cafe2 = db.CAFEs.Single(n => n.MaCafe == cafe.MaCafe);
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                DateTime now = DateTime.Now;
                string date_str = now.ToString("yyyyMMdd_HHmmss");
                var path = Path.Combine(Server.MapPath("~/image products"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                cafe.Anh = fileName;
                cafe2.Anh = cafe.Anh;
            }
            //Luu vao CSDL
            cafe2.TenCafe = cafe.TenCafe;
            cafe2.Gia = cafe.Gia;
            cafe2.MoTa = cafe.MoTa;
            if (cafe.NgayCapNhat != null)
            {
                cafe2.NgayCapNhat = cafe.NgayCapNhat;
            }
            cafe2.MaLoai = cafe.MaLoai;
            db.SubmitChanges();
            return RedirectToAction("Cafe");
        }
        public ActionResult LoaiCafe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.LOAICAFEs.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]

        public ActionResult ThemLoaiCaphe()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemLoaiCaphe(LOAICAFE loaicafe)
        {
            db.LOAICAFEs.InsertOnSubmit(loaicafe);
            db.SubmitChanges();
            return RedirectToAction("LoaiCafe");
        }

        [HttpGet]
        public ActionResult XoaLoai(int id)
        {
            LOAICAFE loaicafe = db.LOAICAFEs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.Maloai = loaicafe.MaLoai;
            if (loaicafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loaicafe);
        }
        [HttpPost, ActionName("XoaLoai")]
        public ActionResult XacNhanXoa(int id)
        {
            LOAICAFE loaicafe = db.LOAICAFEs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.Maloai = loaicafe.MaLoai;
            if (loaicafe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAICAFEs.DeleteOnSubmit(loaicafe);
            db.SubmitChanges();
            return RedirectToAction("LoaiCafe");
        }


    }
}