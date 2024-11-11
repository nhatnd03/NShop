using app1.Helper;
using app1.Models;
using app1.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace app1.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;
        public KhachHangController(Hshop2023Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        #region DangKy
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khackhang = _mapper.Map<KhachHang>(model);
                    khackhang.RandomKey = RandomKeyUtil.GenerateRamdomKey();
                    khackhang.MatKhau = model.MatKhau.ToMd5Hash(khackhang.RandomKey);
                    khackhang.HieuLuc = true;//dung mail de xac nhan
                    khackhang.VaiTro = 0;
                    if (Hinh != null)
                    {
                        khackhang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                    }
                    db.KhachHangs.Add(khackhang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {

                    ViewBag.message = "co loi:" + ex.Message;
                }
            }
            return View();
        }
        #endregion
        #region DangNhap
        [HttpGet]
        public IActionResult DangNhap(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        public async Task<IActionResult> DangNhap(LoginVM model, string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var khachhang = db.KhachHangs.FirstOrDefault(kh => kh.MaKh == model.MaKh);
                if (khachhang == null)
                {
                    ModelState.AddModelError("Lỗi", "Không tồn tại khách hàng");
                }
                else
                {
                    if (!khachhang.HieuLuc)
                    {
                        ModelState.AddModelError("Lỗi", "Tài khoản đã bị khóa, vui lòng liên hệ admin:0382966829");
                    }
                    else
                    {
                        if (khachhang.MatKhau != model.MatKhau.ToMd5Hash(khachhang.RandomKey))
                        {
                            ModelState.AddModelError("Lỗi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.Email, khachhang.Email),
                                new Claim(ClaimTypes.Name, khachhang.HoTen),
                                new Claim("CustomerId", khachhang.MaKh),
                                new Claim(ClaimTypes.Role, "Customer ")
                            };
                            var claimIdenity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimPricipal = new ClaimsPrincipal(claimIdenity);
                            await HttpContext.SignInAsync(claimPricipal);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }

            }
            return View();
        }
        #endregion
        [Authorize]
        public IActionResult profile()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();   
            return Redirect("/");
        }
    }
}
