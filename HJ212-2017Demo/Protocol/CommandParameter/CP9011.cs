using HJ212_2017Demo.Protocol.Enum;

namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 请求应答
    /// </summary>
    public class CP9011 : CP
    {
        /// <summary>
        /// 请求应答
        /// </summary>
        public RequestResult? QnRtn { get; set; }
    }
}
