namespace HJ212_2017Demo.Protocol.CommandParameters
{
    /// <summary>
    /// 设置现场机访问密码
    /// </summary>
    public class CP1072 : CP
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPW { get; set; }
    }
}
