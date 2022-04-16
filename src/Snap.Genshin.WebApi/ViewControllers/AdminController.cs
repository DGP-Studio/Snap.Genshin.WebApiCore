using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.WebApi.Models;
using Snap.Genshin.WebApi.Services;
using Snap.Genshin.WebApi.Utilities;
using System.Security.Claims;

namespace Snap.Genshin.WebApi.ViewControllers
{
    public class AdminController : Controller
    {
        public AdminController(IConfiguration configuration, KeyValueConfigService keyValueConfig)
        {
            this.configuration = configuration;
            this.keyValueConfig = keyValueConfig;
        }

        private readonly IConfiguration configuration;
        private readonly KeyValueConfigService keyValueConfig;

        [HttpGet]
        public IActionResult BasicConfig()
        {
            var isLogin = HttpContext.Session.GetString("_IsLogin");
            if (isLogin == "true")
            {
                var model = new BasicConfigViewModel
                {
                    Manifesto = keyValueConfig.GetString(StoredConfigKeys.Manifesto),
                };
                return View(model);
            }
                
            else
                return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBasicConfig(BasicConfigViewModel model)
        {
            keyValueConfig.SetString(StoredConfigKeys.Manifesto, model.Manifesto);
            ViewBag.Message = "配置保存成功！";
            return View("BasicConfig");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    
                    this.SignIn(User, JwtBearerDefaults.AuthenticationScheme);
                    return RedirectToAction("BasicConfig");
                }
            }

            return View();
        }
    }
}
