using app1.Models;
using app1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app1.ViewComponents
{
    public class FeaturedProductsViewComponent:ViewComponent
    {
        private readonly Hshop2023Context _context;
        public FeaturedProductsViewComponent(Hshop2023Context context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var featuredProducts = _context.HangHoas.Include(p => p.MaLoaiNavigation).OrderByDescending(p => p.DonGia ?? 0).Take(3).Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).ToList();
            return View(featuredProducts);
         }
    }
}
