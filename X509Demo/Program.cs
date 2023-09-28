using Opc.Ua;
using System;
using System.Net.Sockets;
using X509Demo.OpcUaHelper;

namespace X509Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("输入opcua服务器Ip（直接输入Enter则默认10.135.161.130）：");
                var x = Console.ReadLine();
                Console.WriteLine("输入opcua服务器端口号（直接输入Enter则默认4840）：");
                var y = Console.ReadLine();
                string ip = string.IsNullOrEmpty(x) ? "10.135.161.130" : x;
                int port = string.IsNullOrEmpty(y) ? 4840 : int.Parse(y);

                var Client = new OpcUaClient();

                // 是否使用安全连接
                Client.UseSecurity = true;

                Console.WriteLine($"正在连接 opc.tcp://{ip}:{port} - " +
                    $"[{MessageSecurityMode.SignAndEncrypt}#{SecurityPolicies.GetDisplayName(SecurityPolicies.Basic256)}]");
                Client.ConnectServer($"opc.tcp://{ip}:{port}", MessageSecurityMode.SignAndEncrypt, SecurityPolicies.Basic256).Wait();


                if (Client.Connected)
                {
                    Console.WriteLine("连接成功!");
                    Console.WriteLine("尝试关闭连接。。");
                    Client.Disconnect();
                    Console.WriteLine("已关闭!");
                }
                else
                {
                    Console.WriteLine("连接失败。。。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常：\r\n{ex}");
            }

            Console.WriteLine("按任意键退出...");
            Console.Read();
        }
    }
}
