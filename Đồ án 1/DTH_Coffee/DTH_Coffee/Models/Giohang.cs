using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTH_Coffee.Models;

namespace DTH_Coffee.Models
{
    public class Giohang
    {
        dbQLQuanCafeDataContext data = new dbQLQuanCafeDataContext();
        
        public int iMaCafe { set; get;}
        public string sTenCafe { set; get; }
        public string sAnh { set; get; }
        public int  iDongia{ set; get; }
        public int iSoluong { set; get; }
        public int iThanhtien
        {
            get { return iSoluong * iDongia; } 
        }

        public Giohang(int MaCafe)
        {
            iMaCafe = MaCafe;
            CAFE cafe = data.CAFEs.Single(n => n.MaCafe == iMaCafe);
            sTenCafe = cafe.TenCafe;
            sAnh = cafe.Anh;
            iDongia = int.Parse(cafe.Gia.ToString());
            iSoluong = 1;
        }

    }
}