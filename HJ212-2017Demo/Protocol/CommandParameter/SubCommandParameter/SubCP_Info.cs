namespace HJ212_2017Demo.Protocol.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 现场机信息
    /// </summary>
    public class SubCP_Info : SubCP
    {
        /// <summary>
        /// 现场端信息
        /// </summary>
        public string Info { get; set; }


        public SubCP_Info(string name) : base(name) { }
    }
}
