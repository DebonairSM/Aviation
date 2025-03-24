using Microsoft.AspNetCore.Mvc;

namespace AzureMicroservicesPlatform.Services.Aircraft.Controllers
{
    public class IdentityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
