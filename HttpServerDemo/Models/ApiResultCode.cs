namespace HttpServerDemo.Models
{
    /// <summary>
    /// Api结果状态码.
    /// </summary>
    public enum ApiResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 1,

        /// <summary>
        /// 异常
        /// </summary>
        Error = 510,

        /// <summary>
        /// 无Token
        /// </summary>
        NoToken = 430,

        /// <summary>
        /// Token超时
        /// </summary>
        TokenTimeout = 431,

        /// <summary>
        /// Token验证失败
        /// </summary>
        TokenInvalid = 432,
    }
}
