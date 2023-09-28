using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqttDemo
{
    public static class MqttClientDemo
    {
        /// <summary>
        /// 启动一个客户端并订阅消息.
        /// </summary>
        public static async void StartClientAsyn()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();
            var optionbuilder = new MqttClientOptionsBuilder()
                .WithClientId("5.Gateway.pdev.sany.mdc.S0502RootCloudAGV01")
                .WithTcpServer("10.0.92.23", 9993)
                .WithCredentials("S0502RootCloudAGV01", "fc38d236a6bc4d13b5e002f5e71da9ea")
                .WithTls(new MqttClientOptionsBuilderTlsParameters()
                {
                    UseTls = true,
                    AllowUntrustedCertificates = true,
                    CertificateValidationHandler = (a) => true
                });
            var tokenConn = new CancellationToken(false);
            await mqttClient.ConnectAsync(optionbuilder.Build(), tokenConn);

            var tokenSub = new CancellationToken(false);
            var sub = await mqttClient.SubscribeAsync(new MqttClientSubscribeOptions()
            {
                TopicFilters = new List<MqttTopicFilter>
                {
                    new MqttTopicFilter()
                    {
                        Topic = "$SANY/gateway/pdev/sany/mdc/S0502holi02/data",
                        QualityOfServiceLevel=0
                    }
                }
            }, tokenSub);
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate((e) =>
            {
                if (e.ApplicationMessage.Payload != null)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                }
            });
        }
    }
}
