using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTH_Coffee.Models;
using PagedList;
using PagedList.Mvc;

namespace DTH_Coffee.Controllers
{
    public class DTH_CoffeeController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu db
        dbQLQuanCafeDataContext data = new dbQLQuanCafeDataContext();

        // GET: DTH_Coffee
        public ActionResult Index(int ? page)
        {
            //tao bien quy doi so san pham tren moi trang
            int pageSize = 5;
            //Tao bien so trang
            int pageNum = (page ?? 1);

            var cafemoi = Laycafemoi(16);
            return View(cafemoi.ToPagedList(pageNum, pageSize));
        }

        private List<CAFE> Laycafemoi(int count)
        {
            //Sap xep thoi gian theo ngay cap nhat, lay count dong dau
            return data.CAFEs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        public ActionResult LoaiCafe()
        {
            var loaicafe = from cd in data.LOAICAFEs select cd;
            return PartialView(loaicafe);
        }

        public ActionResult SPTheoLoai(int id)
        {
            var cafe = from s in data.CAFEs where s.MaLoai==id select s;
            return View(cafe);
        }

        //Details
        public ActionResult Details(int id)
        {
            var cafe = from s in data.CAFEs
                       where s.MaCafe == id
                       select s;
            return View(cafe.Single());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }

    }
}