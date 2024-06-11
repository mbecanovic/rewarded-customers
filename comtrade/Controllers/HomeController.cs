using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace comtrade.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
