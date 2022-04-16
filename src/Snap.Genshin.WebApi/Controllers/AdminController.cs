using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.WebApi.Models;

namespace Snap.Genshin.WebApi.Controllers
{
    public class AdminController : Controller
    {
        public AdminController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        public IActionResult Index()
        {
            var isLogin = HttpContext.Session.GetString("_IsLogin");
            if (isLogin == "true")
                return View();
            else
                return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminConfig = configuration.GetSection("Admin");
                var userName = adminConfig.GetValue<string>("Username");
                var password = adminConfig.GetValue<string>("Password");

                if (model.UserName == userName && model.Password == password)
                {
                    HttpContext.Session.SetString("_IsLogin", "true");
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
