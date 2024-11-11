using app1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using app1.Helper;

namespace app1.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
           var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            var count = cart.Count;
            ViewBag.Count = count;
            return View();
        }
    }
}
