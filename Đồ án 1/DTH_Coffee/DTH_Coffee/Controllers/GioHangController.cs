using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTH_Coffee.Models;

namespace DTH_Coffee.Controllers
{
    public class GioHangController : Controller
    {
        dbQLQuanCafeDataContext data = new dbQLQuanCafeDataContext();
        // GET: GioHang
        
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Them sp vao gio hang
        public ActionResult ThemGiohang(int iMaCafe, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMaCafe == iMaCafe);
            if (sanpham==null)
            {
                sanpham = new Giohang(iMaCafe);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Tinh tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        //Tinh tong tien
        private int TongTien()
        {
            int iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang!=null)
            {
                iTongTien = lstGiohang.Sum(n => n.iThanhtien);
            }
            return iTongTien;
        }
        //Xay dung trang gio hang
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "DTH_Coffee");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //Xoa gio hang
        public ActionResult Xoagiohang(int iMaCafe)
        {
            //Lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sp da co trong session gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaCafe == iMaCafe);
            //Neu ton tai thi cho sua so luong
            if (sanpham !=null)
            {
                lstGiohang.RemoveAll(n => n.iMaCafe == iMaCafe);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "");
            }
            return RedirectToAction("GioHang");
        }
        //Cap nhat gio hang
        public ActionResult Capnhatgiohang(int iMaCafe, FormCollection f)
        {
            //Lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra da co trong session "gio hang"
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaCafe == iMaCafe);
            // Neu ton tai thi cho sua so luong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Xoatatca()
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "DTH_Coffee");
        }
        //Hien thi view dathang de cap nhat cac thong tin cho don hang
        [HttpGet]
        public ActionResult Dathang()
        {
            //Kiem tra dang nhap
            if(Session["TK"]== null || Session["TK"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"]== null)
            {
                return RedirectToAction("Index", "DTH_Coffee");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            //Them donhang
            DONDATHANG dh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TK"];
            List<Giohang> gh = Laygiohang();
            dh.MaKH = kh.MaKH;
            dh.NgayDat = DateTime.Now;
            dh.TinhTrang = false;
            dh.DaThanhToan = false;
            data.DONDATHANGs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaCafe = item.iMaCafe;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = item.iDongia;
                data.CHITIETDONHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhan", "GioHang");
        }
        public ActionResult Xacnhan()
        {
            return View();
        }
        
    }
}