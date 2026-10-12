using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.ViewComponents
{
    public class _AdminLayoutMainSelectionViewBagComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
