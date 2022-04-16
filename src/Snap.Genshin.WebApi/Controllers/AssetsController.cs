using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.WebApi.Services;
using Snap.Genshin.WebApi.Utilities;

namespace Snap.Genshin.WebApi.Controllers
{
    /// <inheritdoc/>
    [Route("[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        /// <inheritdoc/>
        public AssetsController(KeyValueConfigService keyValueConfigService)
        {
            this.keyValueConfigService = keyValueConfigService;
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult GetManifesto()
        {
            var manifesto = keyValueConfigService.GetString(StoredConfigKeys.Manifesto);
            return this.Success("公告获取成功", manifesto);
        }

        private readonly KeyValueConfigService keyValueConfigService;
    }
}
