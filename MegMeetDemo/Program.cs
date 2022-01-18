using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MegMeetDemo
{
    internal class Program
    {
        // 网络参数
        static IPAddress _ip;
        static int _port;
        static int _portLocal;

        // 是否使用ascii编解码
        static bool _isASCII = true;
        static string _modeStr = "ASCII模式";

        // tcp通讯参数
        static TcpClient _tcpClient;
        static NetworkStream _tcpStream;
        static Task _task;
        static byte[] _buffer = new byte[1024];

        // udp参数
        static UdpClient _udpClient;
        static IPEndPoint _ipe;

        // 日志记录器
        static ILog _logger;

        static void Main(string[] args)
        {
            InitLogger();

            Console.Write("请输入远程主机IP（默认127.0.0.1）:");
            var str1 = Console.ReadLine();
            _ip = string.IsNullOrWhiteSpace(str1)
                ? IPAddress.Parse("127.0.0.1")
                : IPAddress.Parse(str1);

            Console.Write("请输入远程端口号（默认9332）:");
            var str2 = Console.ReadLine();
            _port = string.IsNullOrWhiteSpace(str2)
                ? 9332
                : int.Parse(str2);
            Console.Write("请输入本机端口号（默认9331）:");
            var str3 = Console.ReadLine();
            _portLocal = string.IsNullOrWhiteSpace(str3)
                ? 9331
                : int.Parse(str3);
            Console.Write("是否将数据转换为ASCII字符，（默认ASCII模式，输入‘n’取消）:");
            var str4 = Console.ReadLine();
            _isASCII = str4.Trim().ToLower() != "n"
                ? true
                : false;
            _modeStr = _isASCII ? "ASCII模式" : "字节模式";

            Console.WriteLine();

            DoUdpCommunication(_ip, _port);
        }

        /// <summary>
        /// 初始化日志记录器
        /// </summary>
        private static void InitLogger()
        {
            var LoggerRepository = LogManager.CreateRepository("Log4netConsolePractice");
            XmlConfigurator.ConfigureAndWatch(LoggerRepository, new FileInfo("App.config"));

            _logger = LogManager.GetLogger(LoggerRepository.Name, "default");
        }


        #region TCP通讯

        /// <summary>
        /// 建立一个TCP连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private static void DoTcpCommunication(IPAddress ip, int port)
        {
            Console.WriteLine("===========  开始 TCP 通讯（输入‘q’或‘Q’退出） ===========");

            _tcpClient = new TcpClient(ip.ToString(), port);

            if (_tcpClient.Connected)
            {
                Console.WriteLine("连接成功！");

                _tcpStream = _tcpClient.GetStream();
                Console.WriteLine("获取网络流成功！");
            }

            // 开启接受处理线程
            StartTcpReceiveTask();
            Console.WriteLine("数据接收线程开启！");

            // 主线程用于发送数据
            while (true)
            {
                Console.WriteLine($"输入需要发送的数据（{_modeStr}）：");
                var msg = Console.ReadLine();
                if (msg.ToLower().Trim() == "q")
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                    break;
                }

                TcpSend(msg);
            }
        }

        /// <summary>
        /// 接收函数
        /// </summary>
        static void StartTcpReceiveTask()
        {
            _task = Task.Run(() =>
            {
                while (true)
                {
                    if (_tcpClient != null)
                    {
                        if (_tcpClient.Connected)
                        {
                            int len = _tcpStream.Read(_buffer, 0, _buffer.Length);
                            if (len > 0)
                            {
                                // 解码
                                var msg = _isASCII
                                ? Encoding.ASCII.GetString(_buffer, 0, _buffer.Length)
                                : ToByteString(_buffer.Take(len).ToArray());

                                Console.WriteLine($"<<< {msg}");
                                Log($"<<< {msg}");
                                Array.Clear(_buffer, 0, _buffer.Length);
                            }
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        static void TcpSend(string msg)
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                // 编码
                var data = _isASCII
                    ? Encoding.ASCII.GetBytes(msg, 0, msg.Length)
                    : FromByteString(msg);
                _tcpStream.BeginWrite(data, 0, data.Length, new AsyncCallback((iar) =>
                {
                    string msg = iar.AsyncState as string;
                    Console.WriteLine($"   >>> {msg}");
                    Log($"   >>> {msg}");

                }), msg);
            }
        }

        #endregion


        #region UDP通讯

        /// <summary>
        /// 建立一个Udp通讯
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private static void DoUdpCommunication(IPAddress ip, int port)
        {
            Console.WriteLine("===========  开始 UDP 通讯（输入‘q’或‘Q’退出） ===========");

            var ipeLocal = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _portLocal);
            _udpClient = new UdpClient(ipeLocal);
            _ipe = new IPEndPoint(ip, port);

            // 开启接受处理线程
            StartUdpReceiveTask();
            Console.WriteLine("数据接收线程开启！");

            // 主线程用于发送数据
            while (true)
            {
                Console.WriteLine($"输入需要发送的数据（{_modeStr}）：");
                var msg = Console.ReadLine();
                if (msg.ToLower().Trim() == "q")
                {
                    _udpClient.Close();
                    _udpClient = null;
                    break;
                }

                UdpSend(msg);
            }
        }

        /// <summary>
        /// 接收函数
        /// </summary>
        static void StartUdpReceiveTask()
        {
            _task = Task.Run(() =>
            {
                while (true)
                {
                    if (_udpClient != null)
                    {
                        try
                        {
                            var data = _udpClient.Receive(ref _ipe);
                            if (data.Length > 0)
                            {
                                // 解码
                                var msg = _isASCII
                                ? Encoding.ASCII.GetString(data, 0, data.Length)
                                : ToByteString(data);

                                Console.WriteLine($"<<< {msg}");
                                Log($"<<< {msg}");
                                Array.Clear(_buffer, 0, _buffer.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log("udp异常！", ex);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        static void UdpSend(string msg)
        {
            if (_udpClient != null)
            {
                // 编码
                var data = _isASCII
                    ? Encoding.ASCII.GetBytes(msg, 0, msg.Length)
                    : FromByteString(msg);
                _udpClient.BeginSend(data, data.Length, _ipe, new AsyncCallback((iar) =>
                  {
                      string msg = iar.AsyncState as string;
                      Console.WriteLine($"   >>> {msg}");
                      Log($"   >>> {msg}");
                  }), msg);
            }
        }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private static void Log(string msg, Exception ex = null)
        {
            if (ex == null)
            {
                _logger.Info(msg);
            }
            else
            {
                _logger.Error(msg, ex);
            }
        }

        /// <summary>
        /// 将十六进制字符串转换为所见即所得
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ToByteString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in bytes)
            {
                var ch = b.ToString("X2");
                sb.Append(ch);
                i++;
                if (i % 2 == 0)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转换为所见即所得的字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static byte[] FromByteString(string msg)
        {
            List<byte> bs = new List<byte>();

            var data = msg.ToLower().Replace("0x", "").Replace(" ", "");
            int i = 0;
            while (true)
            {
                var b = Convert.ToByte(data.Substring(i, 2), 16);
                bs.Add(b);

                i += 2;
                if (i > data.Length - 2)
                {
                    break;
                }
            }
            return bs.ToArray();
        }

        #endregion
    }
}
