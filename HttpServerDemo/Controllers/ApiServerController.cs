using HttpServerDemo.Transformers;
using HttpServerDemo.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Net.Mime;

namespace HttpServerDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("any")]
    [ApiController]
    public class ApiServerController : ControllerBase
    {
        /// <summary>
        /// 测试接口.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public IActionResult Test()
        {
            // 样例
            var obj = new ApiDeviceData();
            obj.Code = "1";
            obj.Data = new System.Collections.Generic.List<DeviceValue>()
            {
                new DeviceValue{Address = "address1",Value = "123.456" },
                new DeviceValue{Address = "address2",Value = "true" },
            };
            obj.TimeStamp = DateTime.Now;

            // 序列化
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateTimeConverter());
            options.WriteIndented = true;
            var json = JsonSerializer.Serialize(obj, options);

            var str = Environment.NewLine
                + $"You can send me a string through <http://xxxx:xx/api/apiserver/pushstring>,"
                + Environment.NewLine
                + $"or you can send me a json object through <http://xxxx:xx/api/apiserver/pushdevicedata> in the following format."
                + Environment.NewLine
                + Environment.NewLine
                + json;
            return Ok(str);
        }

        /// <summary>
        /// 上传字符串数据(plain/text).
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        public ApiResultModel<string> PushString([FromBody] object msg)
        {
            var sourceIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var result = new ApiResultModel<string>();
            result.message = "success";
            result.code = (int)ApiResultCode.Success;
            result.data = msg.ToString();
            return result;
        }

        /// <summary>
        /// 上传ApiDeviceData数据(采用Raw Application/Json媒体类型).
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        public ApiResultModel<ApiDeviceData> PushDeviceData([FromBody] ApiDeviceData obj)
        {
            var result = new ApiResultModel<ApiDeviceData>();
            result.message = "success";
            result.code = (int)ApiResultCode.Success;
            result.data = obj;
            return result;
        }
    }
}
