using HJ212_2017Demo.Protocol;
using HJ212_2017Demo.Protocol.CommandParameters;
using HJ212_2017Demo.Protocol.CommandParameters.SubCommandParameter;
using HJ212_2017Demo.Protocol.Enum;
using System;
using System.Collections.Generic;

namespace HJ212_2017Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var msg = "##0343QN=20200826094652892;ST=22;CN=2011;PW=852013;MN=010000A8900016F000169DC0;Flag=4;" +
            //    "CP=&&DataTime=20200826094652;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=3326;A1001-Rtd=8000;A1001-Rtd=8000;A1001-Rtd=8000;" +
            //    "A1001-Rtd=8000;A1001-Rtd=8000;A1001-Rtd=16415;A1001-Rtd=16415;" +
            //    "A1001-Rtd=16415;A1001-Rtd=16415;A1001-Rtd=3326&&A441\r\n";
            //var x = Message.Parse(msg);

            //var customcp = "DataTime=20200826094652;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;werqwerqwerwasdferqqA1001-Rtd=28926289266;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=65436;A1001-Rtd=65336;A1001-Rtd=65236;A1001-Rtd=28926;" +
            //    "A1001-Rtd=3326;A1001-Rtd=8000;A1001-Rtd=8000;A1001-Rtd=8000;" +
            //    "A1001-Rtd=8000;A1001-Rtd=8000;A1001-Rtd=16415;A1001-Rtd=16415;" +
            //    "A1001-Rtd=16415;A1001-Rtd=16415;A1001-Rtd=3326";
            //var a = new Command(SourceType.Slave, SystemType.AIR_QUALITY_MONITORING, 2011, "852013", "010000A8900016F000169DC0", false, customcp);
            //var b = a.Pack();
            //var c = Command.UnPack(b);
            //Console.WriteLine(a);
            //Console.WriteLine(b.First().QN);
            //Console.WriteLine(c);

            //string aaa = null;
            //var bbb = aaa.ValidateCP();

            //var str = "QnRtn=1";
            //var cp = CPBase.Parse<CP9011>(str);
            //var str2 = cp.ToString();

            //var cpdata = new CP2011_Upload()
            //{
            //    DataTime = DateTime.Now,
            //    SubCP = new Dictionary<string, SubCP>()
            //    {
            //        { "w10028", new SubCP("w10018")
            //            {
            //                Rtd = 123.456,
            //                ZsRtd = 123.456,
            //                Flag =DataFlag.M
            //            }
            //        }
            //    }
            //};
            //var cpstr3 = cpdata.ToString();
            //var cp1 = CPBase.Parse<CP2011_Upload>(cpstr3);

            var message = new Message()
            {
                Command = new Command(SourceType.Slave, SystemType.AIR_QUALITY_MONITORING, 2011, "123456", "010000A8900016F000169DC0", false)
                {
                    CP = new CP2011_Upload()
                    {
                        DataTime = DateTime.Now,
                        SubCP = new Dictionary<string, SubCP>()
                        {
                            { "w01008",
                                new SubCP_Rtd("w01008")
                                {
                                    SampleTime = DateTime.Now,
                                    Rtd = 123.456
                                }
                            }
                        }
                    }
                }
            };
            var msg = message.ToString();
            var message2 = Message.Parse(SourceType.Slave, msg);
            var msg2 = message2.ToString();
            Console.WriteLine(msg);
            Console.WriteLine(msg2);
        }
    }
}
