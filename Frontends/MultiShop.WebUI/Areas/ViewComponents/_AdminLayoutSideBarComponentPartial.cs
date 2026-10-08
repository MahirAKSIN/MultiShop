using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.ViewComponents
{
    public class _AdminLayoutSideBarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
