using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using QLBanhang.Models;
using PagedList;
namespace QLBanhang.Controllers
{
	public class HomeController : Controller
	{
		qlbanhangEntities db = new qlbanhangEntities();
		public ActionResult Index(int maloaisp = 0, string SearchString = "")
		{
			if (SearchString != "")
			{
				var sanPhams = db.SanPhams.Include(s => s.LoaiSP).Where(x => x.TenSP.ToUpper().Contains(SearchString.ToUpper()));
				return View(sanPhams.ToList());
			}
			else if (maloaisp == 0)
			{
				var sanPhams = db.SanPhams.Include(s => s.LoaiSP);
				return View(sanPhams.ToList());
			}
			else
			{
				var sanPhams = db.SanPhams.Include(s => s.LoaiSP).Where(x => x.MaLoaiSP == maloaisp);
				return View(sanPhams.ToList());
			}
		}
		private IEnumerable<KeyValuePair<int, string>> GetLoaiSanPham()
		{
			
			return db.LoaiSPs.Select(l => new KeyValuePair<int, string>(l.MaLoaiSP, l.TenLoaiSP)).ToList();
		}
	
	public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}