using MQTTnet;
using System;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Server;
using System.Threading;
using System.Threading.Tasks;

namespace MqttDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // test
            var cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    Console.WriteLine(i++);
                    Thread.Sleep(500);
                }
            }, cts.Token);

            Thread.Sleep(1000);
            cts.Cancel();




            //MqttClientDemo.StartClientAsyn();

            Console.WriteLine("MQTT client运行中...(按Q以结束)");

            while (Console.ReadKey(true).Key != ConsoleKey.Q)
            {
            }
        }
    }
}
