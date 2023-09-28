using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace HttpServerDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MonitorController : ControllerBase
    {
        IServiceProvider _provider;

        public MonitorController(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 测试.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Test()
        {
            Console.WriteLine("test");
            return Content("test");
        }

        /// <summary>
        /// 测试--获取图片.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Image()
        {
            var imgPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? @"data\image.png"
                    : @"data/image.png";
            var imgByte = System.IO.File.ReadAllBytes(imgPath);
            var imgStream = new MemoryStream(imgByte);
            return File(imgStream, "image/png");
        }

        /// <summary>
        /// 博储模拟接口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="appname"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CutSystemState(string ip, string appname)
        {
            string filename = string.Empty;
            if (string.Compare(appname, "tubepro", true) == 0)
                filename = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? @"data\tubepro.txt"
                    : @"data/tubepro.txt";
            else if (string.Compare(appname, "cypcut", true) == 0)
                filename = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? @"data\cypcut.txt"
                    : @"data/cypcut.txt";
            else return null;

            using var sr = new StreamReader(filename);
            var content = sr.ReadToEnd();
            return Content(content);
        }
    }
}
