namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 现场机时间校准请求
    /// </summary>
    public class CP1013 : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }
}
