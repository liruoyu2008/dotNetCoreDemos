﻿namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 启动清洗/反吹
    /// </summary>
    public class CP3013 : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }
}
