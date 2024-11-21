using app1.Models;
using app1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using app1.Helper;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace app1.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;
        public CartController(Hshop2023Context context) { db = context; }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }
        [Authorize]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var giohang = Cart;
            var item = giohang.FirstOrDefault(p=>p.MaHh==id);
            if (item == null)
            {
                var hanghoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hanghoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng mã có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem()
                {
                    MaHh = hanghoa.MaHh,
                    TenHh = hanghoa.TenHh,
                    DonGia = hanghoa.DonGia ?? 0,
                    Hinh = hanghoa.Hinh ?? "",
                    SoLuong = quantity
                };
                giohang.Add(item);
            }
            else 
            {
                item.SoLuong += quantity;
                db.SaveChanges();
            }
            HttpContext.Session.Set(MySetting.CART_KEY, giohang);
            return RedirectToAction("Index");
        } 
        [Authorize]
        public IActionResult Checkout() 
        {
            if(Cart.Count()==0 )
            {
                return Redirect("/");
            }
            return View(Cart);
        }
        //public IActionResult RemoveCart(int id)
        //{ 
        //    var giohang = Cart;
        //    var item = giohang.FirstOrDefault(p => p.MaHh == id);
        //    if (item != null)
        //    { 
        //        giohang.Remove(item);
        //        HttpContext.Session.Set(MySetting.CART_KEY, giohang);
        //    }
        //    return RedirectToAction("Index");
        //}

        //[Authorize]
        //[HttpPost]
        //public IActionResult UpdateQuantity(int id, int quantity)
        //{ 
        //    var giohang = Cart; 
        //    var item = giohang.FirstOrDefault(p => p.MaHh == id); 
        //    if (item != null && quantity > 0) 
        //    { 
        //        item.SoLuong = quantity;


        //        HttpContext.Session.Set(MySetting.CART_KEY, giohang);
        //    }

        //    var subtotal = giohang.Sum(a => a.ThanhTien);
        //    var total = subtotal + 3; // Shipping fee

        //    return Json(new { thanhTien = item.ThanhTien, subtotal, total });
        //}
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var giohang = Cart;
            var item = giohang.FirstOrDefault(p => p.MaHh == id);

            if (item != null && quantity > 0)
            {
                item.SoLuong = quantity;

                // Lưu lại cart vào Session
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);
            }

            var subtotal = giohang.Sum(a => a.SoLuong * a.DonGia);  // Tính lại subtotal
            var total = subtotal + 3; // Phí vận chuyển

            return Json(new { thanhTien = item?.ThanhTien, subtotal, total });
        }

        [HttpPost]
        public IActionResult RemoveCart(int id)
        {
            var giohang = Cart;
            var item = giohang.FirstOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                giohang.Remove(item);
            }

            var subtotal = giohang.Sum(a => a.ThanhTien);
            var total = subtotal + 3; // Shipping fee
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);

            return Json(new { subtotal, total });
        }

    }
}
