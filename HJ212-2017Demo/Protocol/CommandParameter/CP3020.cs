﻿using HJ212_2017Demo.Protocol.CommandParameters.SubCommandParameter;
using System;
using System.Collections.Generic;

namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 提取现场机信息
    /// </summary>
    public class CP3020_Request : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 在线监控（监测）设备信息编码
        /// </summary>
        public string InfoId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 上传现场机信息
    /// </summary>
    public class CP3020_Upload : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime? DataTime { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, SubCP> SubCP { get; set; }
    }
}
