using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpcDemo
{
    class OpcDa
    {
        public static void Main2(string[] args)
        {
            try
            {
                // test
                string ip1 = "10.16.134.200";
                var x1 = new OPCServer();
                var y1 = x1.GetOPCServers(ip1);


                // 读入参数
                string ip = args.Length > 0 ? args[0] : "127.0.0.1";
                string progID = args.Length > 1 ? args[1] : "kepware.KEPServerEX.V6";
                Console.WriteLine($"IP : {ip}");
                Console.WriteLine($"ProgID : {progID}");
                Console.WriteLine();

                var server = new OPCServer();
                Array sers = server.GetOPCServers(ip);
                Console.WriteLine("查询到以下OPC Servers：");
                for (int i = 1; i <= sers.Length; i++)
                {
                    Console.WriteLine(sers.GetValue(i));
                }
                Console.WriteLine();

                server.Connect(progID, ip);
                OPCBrowser browser = server.CreateBrowser();
                browser.ShowBranches();
                Console.WriteLine("查询到以下OPC Branches：");
                foreach (var item in browser)
                {
                    Console.WriteLine(item);
                }

                var group = server.OPCGroups.Add("default");
                browser.ShowLeafs(true);
                Console.WriteLine("查询到以下OPC Branches：");
                OPCItem itemTest = null;
                List<int> ctypes = new List<int>();
                int handle = 1;
                foreach (var item in browser)
                {
                    itemTest = group.OPCItems.AddItem(item.ToString(), handle++);
                    ctypes.Add(itemTest.CanonicalDataType);
                    if (itemTest.CanonicalDataType > 8211)
                    {
                        Console.WriteLine(itemTest.ItemID);
                    }
                    if (itemTest.ItemID == "数据类型示例.16 位设备.K 寄存器.LLong4")
                    {
                        Console.WriteLine("LLong：");
                        Console.WriteLine(itemTest.CanonicalDataType);
                    }
                    if (itemTest.ItemID == "数据类型示例.16 位设备.K 寄存器.QWord4")
                    {
                        Console.WriteLine("QWord：");
                        Console.WriteLine(itemTest.CanonicalDataType);
                    }
                }
                Console.WriteLine();

                Console.WriteLine("共有以下几种数据类型：");
                var x = ctypes.Distinct();
                foreach (var item in x)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常 : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
