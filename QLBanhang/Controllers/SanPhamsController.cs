using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBanhang.Models;
using System.IO;
namespace QLBanhang.Controllers
{
    public class SanPhamsController : Controller
    {
        private qlbanhangEntities db = new qlbanhangEntities();

		// GET: SanPhams
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

		// GET: SanPhams/Details/5
		public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,Donvitinh,Dongia,MaLoaiSP,HinhSP")] SanPham sanPham , HttpPostedFileBase HinhSP)
        {
            if (ModelState.IsValid)
            {
				if (HinhSP!=null && HinhSP.ContentLength > 0)
				{
					string filename = Path.GetFileName(HinhSP.FileName);
					string path = Server.MapPath("~/Images/" + filename);
					sanPham.HinhSP = "Images/" + filename;
					HinhSP.SaveAs(path);
				}
				db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

       
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Donvitinh,Dongia,MaLoaiSP,HinhSP")] SanPham sanPham, HttpPostedFileBase HinhUpLoad,string HinhSP)
        {
            if (ModelState.IsValid)
            {
				if (HinhUpLoad != null && HinhUpLoad.ContentLength > 0)
				{
					string filename = Path.GetFileName(HinhUpLoad.FileName);
					string path = Server.MapPath("~/Images/" + filename);
					sanPham.HinhSP = "Images/" + filename;
					HinhUpLoad.SaveAs(path);
				}
				else
				{
					sanPham.HinhSP = HinhSP;
				}
				db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
			System.IO.File.Delete(Server.MapPath("~/") + "sanPham.hinhSP");
			return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
