using app1.Models;
using app1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace app1.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;
        public HangHoaController(Hshop2023Context db)
        {
            this.db = db;
        }

        public IActionResult Index(int? loai, int? page, double? minPrice, double? maxPrice)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            // Áp dụng lọc giá nếu có giá trị minPrice và maxPrice
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.DonGia >= minPrice && p.DonGia <= maxPrice);
            }


            var pageNumber = page ?? 1;
            var pageSize = 9; // Số sản phẩm trên mỗi trang

            var rs = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            // Sắp xếp theo giá nếu có lọc giá
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                rs = rs.OrderBy(p => p.DonGia);
            }

            var pagedHangHoas = rs.ToPagedList(pageNumber, pageSize);

            return View(pagedHangHoas);
        }

        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var rs = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).ToList();
            return View(rs);
        }

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .FirstOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var rs = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                DiemDanhGia = 5, //check sau
                Hinh = data.Hinh ?? "",
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10
            };
            return View(rs);
        }

    }
}
