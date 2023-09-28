using MQTTnet;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Server;
using System;
using System.Threading.Tasks;

namespace MqttDemo
{
    public static class MqttServerDemo
    {
        /// <summary>
        /// 启动一个MqttServer.
        /// </summary>
        public static Task StartServerAsyn()
        {
            var serverLogger = new MqttNetEventLogger();
            serverLogger.LogMessagePublished += Logger_LogMessagePublished;
            var server = new MqttFactory().CreateMqttServer(serverLogger);
            var optionbuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(3883) //默认端口是1883,这里可以自己设置
                .WithConnectionValidator((valid) =>
                {
                    Console.WriteLine(valid);
                });

            return server.StartAsync(optionbuilder.Build());
        }

        private static void Logger_LogMessagePublished(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            Console.WriteLine($"系统日志:{e.LogMessage}");
        }
    }
}
