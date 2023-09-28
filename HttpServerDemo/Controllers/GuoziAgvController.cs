using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace HttpServerDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuoziAgvController : ControllerBase
    {
        IServiceProvider _provider;

        public GuoziAgvController(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 国子agv接口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="appname"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public IActionResult get_agv_status([FromBody]object a)
        {
            var filename = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? @"data\guozi.txt"
                : @"data/guozi.txt";
            using var sr = new StreamReader(filename);
            var content = sr.ReadToEnd();
            return Content(content);
        }
    }
}
