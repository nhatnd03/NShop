using System.ComponentModel.DataAnnotations;

namespace app1.ViewModels
{
    public class RegisterVM
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string MaKh { get; set; } 
        [Required(ErrorMessage ="*")]
        [MaxLength(50)]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(50,ErrorMessage ="Tối đa 50 ký tự")]
        [Display(Name = "Họ Tên")]

        public string HoTen { get; set; } 
        [Required(ErrorMessage = "*")]
        [Display(Name = "Giới tính")]

        public bool GioiTinh { get; set; } = true;
        [Display(Name = "Ngày sinh")]

        public DateTime? NgaySinh { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 ký tự")]
        [Display(Name = "Địa chỉ")]

        public string DiaChi { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Số điện thoại")]

        public string DienThoai { get; set; }
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        [Display(Name ="Email")]

        public string Email { get; set; }
        [Display(Name = "Hình ảnh")]

        public string? Hinh { get; set; }

        //public bool HieuLuc { get; set; }

        //public int VaiTro { get; set; }

        //public string? RandomKey { get; set; }
    }
}
