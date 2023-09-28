namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 设置实时数据间隔
    /// </summary>
    public class CP1062 : CP
    {
        /// <summary>
        /// 实时采样数据上报间隔
        /// </summary>
        public ushort? RtdInterval { get; set; }
    }
}
