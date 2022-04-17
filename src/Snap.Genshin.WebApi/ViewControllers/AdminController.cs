using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.WebApi.Models;
using Snap.Genshin.WebApi.Services;
using Snap.Genshin.WebApi.Utilities;
using System.ComponentModel.DataAnnotations;
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
                return LocalRedirect("/Admin/Login?action=BasicConfig");
        }

        [HttpGet]
        public IActionResult MetadataConfig()
        {
            var isLogin = HttpContext.Session.GetString("_IsLogin");
            if (isLogin == "true")
            {
                var model = new MetadataViewModel();
                return View(model);
            }

            else
                return LocalRedirect("/Admin/Login?action=MetadataConfig");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBasicConfig(BasicConfigViewModel model)
        {
            keyValueConfig.SetString(StoredConfigKeys.Manifesto, model.Manifesto);
            ViewBag.Message = "配置保存成功！";
            return View("BasicConfig");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveMetadataConfig([Required, FromForm]IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = $"未选择文件！";
                return View("MetadataConfig");
            }
            ViewBag.Message = $"{file.FileName}更新成功！";
            return View("MetadataConfig");
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string action = "BasicConfig")
        {
            ViewBag.Action = action;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, [FromQuery] string action = "BasicConfig")
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
                    return RedirectToAction(action);
                }
            }

            return View();
        }
    }
}
