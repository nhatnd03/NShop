using app1.Models;
using app1.ViewModels;
using AutoMapper;

namespace app1.Helper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles() 
        {
            //map các thành phần từ RegisterVM sang KhachHang, map các thành phần cùng tên
            CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh=>kh.HoTen, option =>option.MapFrom(RegisterVM=>RegisterVM.HoTen))
                //.ReverseMap();
        }
    }
}
