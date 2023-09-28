using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MegMeetDemo
{
    internal class Program
    {
        // 通讯参数
        static string _ip = "0";

        // udp参数
        static UdpClient _udpClient;
        static IPEndPoint _remoteEP;

        // 焊接参数
        static Parameters _params;

        // 日志记录器
        static ILog _logger;

        static void Main(string[] args)
        {
            InitLogger();

            _params = new Parameters();
            _remoteEP = _ip == "0" ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.Parse(_ip), 0);
            DoUdpCommunication();

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

        /// <summary>
        /// 建立一个Udp通讯
        /// </summary>
        private static void DoUdpCommunication()
        {
            Console.WriteLine("===========  开始 UDP 通讯（输入‘q’或‘Q’退出） ===========");

            // 绑定本地端口
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 3005));

            // 开启接受处理线程
            var task = BeginReceive();
            Console.WriteLine("开始接受udp报文...");

            // 按q键退出
            while (true)
            {
                var msg = Console.ReadKey(true);
                if (msg.Key == ConsoleKey.Q)
                {
                    _udpClient.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        static Task BeginReceive()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    if (_udpClient != null)
                    {
                        try
                        {
                            var data = _udpClient.Receive(ref _remoteEP);

                            // 报文处理
                            OnReceiveData(data);
                        }
                        catch (Exception ex)
                        {
                            Log("udp异常！", ex);
                            Console.WriteLine($"发生异常：{ex}");
                            break;
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
        /// 收到数据的处理办法
        /// </summary>
        /// <param name="frame"></param>
        private static void OnReceiveData(byte[] frame)
        {
            if (frame.Length != 85)
            {
                Console.WriteLine("报文长度不正确.");
                return;
            }

            if (frame[0] != 0x7f || frame[1] != 0x7f || frame[2] != 0x7f || frame[3] != 0x7f
                || frame[81] != 0x7e || frame[82] != 0x7e || frame[83] != 0x7e || frame[84] != 0x7e)
            {
                Console.WriteLine("报文标识不正确.");
                return;
            }

            var data1 = frame.Skip(52).Take(8).ToArray();
            var data2 = frame.Skip(71).Take(8).ToArray();

            if (data1[0] != 0x40 || data2[0] != 0x41)
            {
                Console.WriteLine("报文序号不正确.");
                return;
            }

            _params.ErrorCode1 = data1[2];
            _params.ErrorCode2 = data1[3];

            _params.SetCurrent = (ushort)((data1[4] << 8) + data1[5]);
            _params.SetVoltage = (ushort)((data1[6] << 8) + data1[7]);

            _params.WireSpeed = (ushort)((data2[2] << 8) + data2[3]);
            _params.Current = (ushort)((data2[4] << 8) + data2[5]);
            _params.Voltage = (ushort)((data2[6] << 8) + data2[7]);

            var ps = typeof(Parameters).GetProperties();
            foreach (var p in ps)
            {
                Console.WriteLine($"【{p.Name}】-【{p.GetValue(_params)}】");
            }
        }

        /// <summary>
        /// 收到数据的处理办法2
        /// </summary>
        /// <param name="data"></param>
        private static void OnReceiveData2(byte[] data)
        {
            Console.WriteLine(data.ToHexString());
        }

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
    }
}
