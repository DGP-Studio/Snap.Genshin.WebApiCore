using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.WebApi.Utilities;

namespace Snap.Genshin.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("[Action]")]
        public IActionResult Status()
        {
            return this.Success("snap.genshin.api");
        }
    }
}
