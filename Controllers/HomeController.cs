using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RandomDataGenerator.Code;

namespace RandomDataGenerator.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["LocaleList"] = new SelectList(StaticData._regions, "Value", "Name", Region.EN);
            return View();
        }
    }
}
