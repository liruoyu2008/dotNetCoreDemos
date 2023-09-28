using System;

namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 上传数采仪开机时间
    /// </summary>
    public class CP2081 : CP
    {
        /// <summary>
        /// 数据时间信息
        /// </summary>
        public DateTime? DataTime { get; set; }

        /// <summary>
        /// 数采仪开机时间
        /// </summary>
        public DateTime? RestartTime { get; set; }
    }
}
