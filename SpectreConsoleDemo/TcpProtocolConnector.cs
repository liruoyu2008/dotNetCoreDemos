using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace SpectreConsoleDemo
{
    public class TcpProtocolConnector : IDisposable
    {
        /// <summary>
        /// 连接器的IP地址.
        /// </summary>
        public IPAddress IP { get; private set; }

        /// <summary>
        /// 连接器的设定端口.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 连接状态.
        /// </summary>
        public bool Connected { get { return Client?.Connected == true; } }

        /// <summary>
        /// TcpClient.
        /// </summary>
        public TcpClient Client { get; private set; }


        public TcpProtocolConnector(string ip, int port)
        {
            IP = IPAddress.Parse(ip);
            Port = port;
            Client = new TcpClient();
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            Client.Connect(IP, Port);
            return Client.Connected;
        }

        public int Send(byte[] data)
        {
            return Client.Client.Send(data);
        }

        public void Dispose()
        {
            try
            {
                Client?.Close();
            }
            catch (Exception) { }
        }

        public delegate void ReceivedEventHandler(object sender, ReceivedEventArgs e);
        /// <summary>
        /// Received事件，在套接字收到数据后触发.
        /// </summary>
        public event ReceivedEventHandler OnReceived;
    }
}