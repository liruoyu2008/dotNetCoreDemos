using Modbus.Device;
using System;
using System.Net.Sockets;

namespace ModbusDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // test 
            Console.WriteLine(377);
            Console.ReadLine();

            Console.WriteLine("Hello World!");

            var tcpClient = new TcpClient();
            tcpClient.Connect("192.168.0.6", 502);
            var master = ModbusIpMaster.CreateIp(tcpClient);
            var res = master.ReadCoils(1280, 1);
            foreach (var item in res)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }


    }
}
