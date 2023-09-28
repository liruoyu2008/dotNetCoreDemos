namespace HttpServerDemo.Models
{
    /// <summary>
    /// API返回规范（code=0 表示成功，否则表示失败）
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ApiResultModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private int _code = (int)ApiResultCode.Fail;

        /// <summary>
        /// 数据
        /// </summary>
        private T _data;

        /// <summary>
        /// 消息
        /// </summary>
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResultModel{T}"/> class.
        /// 构造函数，根据参数返回
        /// </summary>
        /// <param name="apiResultCode">状态码</param>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        public ApiResultModel(ApiResultCode apiResultCode, T data, string message)
        {
            _code = (int)apiResultCode;
            _data = data;
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResultModel{T}"/> class.
        /// 默认返回失败，可传状态码
        /// </summary>
        /// <param name="apiResultCode">状态码</param>
        public ApiResultModel(ApiResultCode apiResultCode = ApiResultCode.Fail)
        {
            _code = (int)apiResultCode;
            _data = default;
            _message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResultModel{T}"/> class.
        /// 默认返回，传自定义状态码
        /// </summary>
        /// <param name="customerCode">自定义状态码</param>
        public ApiResultModel(int customerCode)
        {
            _code = customerCode;
            _data = default;
            _message = string.Empty;
        }

        /// <summary>
        /// 获取或设置状态码.
        /// </summary>
        public int code { get => _code; set => _code = value; }

        /// <summary>
        /// 获取或设置数据.
        /// </summary>
        public T data { get => _data; set => _data = value; }

        /// <summary>
        /// 获取或设置消息.
        /// </summary>
        public string message { get => _message; set => _message = value; }
    }
}
