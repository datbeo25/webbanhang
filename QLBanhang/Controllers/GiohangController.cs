using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanhang.Models;
using System.Net;
using System.Net.Mail;

namespace QLBanhang.Controllers
{
	public class GiohangController : Controller
	{
		private qlbanhangEntities db = new qlbanhangEntities();

		public ActionResult Index()
		{
			List<CartItem> giohang = Session["giohang"] as List<CartItem>;
			return View(giohang);
		}

		public RedirectToRouteResult AddToCart(string MaSP)
		{
			if (Session["giohang"] == null)
			{
				Session["giohang"] = new List<CartItem>();
			}

			List<CartItem> giohang = Session["giohang"] as List<CartItem>;

			if (giohang.FirstOrDefault(m => m.MaSP == MaSP) == null)
			{
				SanPham sp = db.SanPhams.Find(MaSP);
				CartItem newItem = new CartItem
				{
					MaSP = MaSP,
					TenSP = sp.TenSP,
					SoLuong = 1,
					DonGia = Convert.ToDouble(sp.Dongia)
				};
				giohang.Add(newItem);
			}
			else
			{
				CartItem cardItem = giohang.FirstOrDefault(m => m.MaSP == MaSP);
				cardItem.SoLuong++;
			}

			Session["giohang"] = giohang;
			return RedirectToAction("Index");
		}

		public RedirectToRouteResult Update(string MaSP, int txtSoluong)
		{
			List<CartItem> giohang = Session["giohang"] as List<CartItem>;
			CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
			if (item != null)
			{
				item.SoLuong = txtSoluong;
				Session["giohang"] = giohang;
			}
			return RedirectToAction("Index");
		}

		public RedirectToRouteResult DelCartItem(string MaSP)
		{
			List<CartItem> giohang = Session["giohang"] as List<CartItem>;
			CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
			if (item != null)
			{
				giohang.Remove(item);
				Session["giohang"] = giohang;
			}
			return RedirectToAction("Index");
		}

		public ActionResult Order(string TenKH, string DiaChi, string Email, string Phone)
		{
			List<CartItem> giohang = Session["giohang"] as List<CartItem>;
			if (giohang == null || giohang.Count == 0)
			{
				return RedirectToAction("Index");
			}

			
			string sMsg = "<html><body>";
			sMsg += "<h2>Thông tin khách hàng</h2>";
			sMsg += "<p><strong>Tên khách hàng:</strong> " + TenKH + "</p>";
			sMsg += "<p><strong>Địa chỉ:</strong> " + DiaChi + "</p>";
			sMsg += "<p><strong>Email:</strong> " + Email + "</p>";
			sMsg += "<p><strong>Điện thoại:</strong> " + Phone + "</p>";

			
			sMsg += "<h2>Thông tin đặt hàng</h2>";
			sMsg += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
			sMsg += "<tr><th style='border: 1px solid black; padding: 8px;'>STT</th><th style='border: 1px solid black; padding: 8px;'>Tên hàng</th><th style='border: 1px solid black; padding: 8px;'>Số lượng</th><th style='border: 1px solid black; padding: 8px;'>Đơn giá</th><th style='border: 1px solid black; padding: 8px;'>Thành tiền</th></tr>";
			int i = 0;
			double tongtien = 0;

			foreach (CartItem item in giohang)
			{
				i++;
				sMsg += "<tr>";
				sMsg += "<td style='border: 1px solid black; padding: 8px; text-align:center;'>" + i.ToString() + "</td>";
				sMsg += "<td style='border: 1px solid black; padding: 8px;'>" + item.TenSP + "</td>";
				sMsg += "<td style='border: 1px solid black; padding: 8px; text-align:center;'>" + item.SoLuong.ToString() + "</td>";
				sMsg += "<td style='border: 1px solid black; padding: 8px;'>" + item.DonGia.ToString("N0") + " VNĐ</td>";
				sMsg += "<td style='border: 1px solid black; padding: 8px;'>" + String.Format("{0:#,###}", item.SoLuong * item.DonGia) + " VNĐ</td>";
				sMsg += "</tr>";
				tongtien += item.SoLuong * item.DonGia;
			}

			sMsg += "<tr><th colspan='4' style='border: 1px solid black; padding: 8px; text-align:right;'>Tổng cộng</th>";
			sMsg += "<th style='border: 1px solid black; padding: 8px;'>" + String.Format("{0:#,### vnđ}", tongtien) + "</th></tr>";
			sMsg += "</table>";
			sMsg += "</body></html>";

			
			MailMessage mail = new MailMessage("nguyenminhdat2562003@gmail.com", Email, "Thông tin đơn hàng", sMsg);
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential("nguyenminhdat2562003@gmail.com", "zwmm ptau mkhy kyud") 
			};

			mail.IsBodyHtml = true;

			try
			{
				client.Send(mail);
			}
			catch (SmtpException ex)
			{
			
				Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
				
			}

			Session["giohang"] = null;

			return RedirectToAction("Index", "Home");
		}


	}
}




