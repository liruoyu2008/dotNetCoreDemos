using System;
using System.Collections.Generic;

namespace HttpServerDemo.Models
{
    /// <summary>
    /// API通道设备一次传输的数据格式
    /// </summary>
    public class ApiDeviceData
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备数据
        /// </summary>
        public List<DeviceValue> Data { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }

    /// <summary>
    /// 设备点位值
    /// </summary>
    public class DeviceValue
    {
        public string Address { get; set; }

        public string Value { get; set; }
    }
}
