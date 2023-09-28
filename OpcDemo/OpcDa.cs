using OPCAutomation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpcDemo
{
    public class OpcDa
    {
        static string kep_server = "Kepware.KEPServerEX.V6";
        static string supcon_server = "SUPCON.JXServer.1";
        static string ip = "127.0.0.1";
        static string kep_item = "通道 1.设备 1.标记 00001";
        static string supcon_item = "fjys_A";

        public static void Main2(string[] args)
        {
            var s = new OPCServer();
            s.Connect(kep_server, ip);
            OutputTree(s.CreateBrowser(), 4, 70000, 200);


            // Create6DeviceForOpcDa();

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        /// <summary>
        /// 创建一颗opc点位树
        /// </summary>
        /// <param name="progid"></param>
        /// <param name="ip"></param>
        private static void MakeOPCTree(string progid, string ip)
        {

            // var b = server.CreateBrowser();
            //OutputTree(b, 0, 70001, 200);
        }

        /// <summary>
        /// 订阅一个示例节点
        /// </summary>
        /// <param name="progid"></param>
        /// <param name="ip"></param>
        /// <param name="itemid"></param>
        private static void DatachangeTest(string progid, string ip, string itemid)
        {
            try
            {
                OPCServer server = new OPCServer();
                server.Connect(progid, ip);

                var g = server.OPCGroups.Add("default");
                var x = g.OPCItems.AddItem(itemid, 1);
                g.DataChange += G_DataChange;
                g.IsActive = true;
                g.IsSubscribed = true;

                server.ServerShutDown += Server_ServerShutDown;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 服务关闭通知
        /// </summary>
        /// <param name="Reason"></param>
        private static void Server_ServerShutDown(string Reason)
        {
            Console.WriteLine("检测到服务已关闭");
        }

        /// <summary>
        /// 值更新
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private static void G_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                Console.WriteLine(ItemValues.GetValue(i));
            }
        }


        #region Megmeet Test

        public static void Create400DeviceForMegmeet()
        {
            using (var sw1 = new StreamWriter(@"C:\Users\Ryu\Desktop\400devices.cfg", false))
            using (var sw2 = new StreamWriter(@"C:\Users\Ryu\Desktop\400Address.txt", false))
            {
                for (int i = 0; i < 999; i++)
                {
                    var no = (i + 1).ToString().PadLeft(3, '0');
                    var channelid = Guid.NewGuid().ToString().Replace("-", "");
                    var maccode = $"{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}{no}";
                    var deviceid = Guid.NewGuid().ToString().Replace("-", "");
                    var devicename = $"megmeet_test_{no}";
                    sw1.Write(CreateSingleDeviceChannelForMegmeet(channelid, maccode, deviceid, devicename));
                    sw1.WriteLine(",");
                    sw2.Write(CreateDeviceAddressConfigForMegmeet(deviceid));
                    sw2.WriteLine(",");
                }
            }
        }

        /// <summary>
        /// 返回一个单设备通道的字符串配置.
        /// </summary>
        /// <param name="channelid"></param>
        /// <param name="maccode"></param>
        /// <param name="deviceid"></param>
        /// <param name="devicename"></param>
        /// <returns></returns>
        public static string CreateSingleDeviceChannelForMegmeet(string channelid, string maccode, string deviceid, string devicename)
        {
            var res = @"{
			    ""ChannelName"": ""(channelid)"",
			    ""ChannelType"": ""MegmeetUdpServer"",
			    ""ConnectParamList"": [
				    {
					    ""ParaItemName"": ""MacCode"",
					    ""ParaItemValue"": ""(maccode)""
				    },
				    {
					    ""ParaItemName"": ""Port"",
					    ""ParaItemValue"": ""3005""
				    }
			    ],
			    ""DeviceList"": [
				    {
					    ""DeviceParamList"": [],
					    ""DeviceType"": ""MegmeetWelder"",
					    ""DeviceName"": ""(devicename)"",
					    ""DeviceID"": ""(deviceid)"",
					    ""ConfigTime"": 1665455127884,
					    ""Explain"": """",
                        ""StationNumber"": ""1"",
					    ""CompanyName"": ""Megmeet"",
					    ""Model"": ""Welder"",
					    ""ProtocolType"": ""MegmeetUdpClient"",
					    ""TimeParam"": {
                    ""ReconDelay"": ""3000"",
						    ""SendMsgDelay"": ""0"",
						    ""ConTimeout"": ""3000"",
						    ""ScanIntervalTime"": ""1000""

                        },
					    ""DeviceCode"": ""(devicename)"",
					    ""ChildCompanyName"": ""(devicename)"",
					    ""Factory"": ""(devicename)"",
					    ""WorkCenter"": ""(devicename)"",
					    ""WorkGroup"": ""(devicename)"",
					    ""Status"": ""Active"",
					    ""CustomerParamList"": [],
					    ""TransmitStatus"": ""Enable"",
					    ""CollectionType"": ""LowFrequency"",
					    ""IsOpenDc"": true,
					    ""IsOpenDnc"": true

                    }
			    ]
		    }";
            res = res.Replace("(channelid)", channelid)
                     .Replace("(maccode)", maccode)
                     .Replace("(deviceid)", deviceid)
                     .Replace("(devicename)", devicename);
            return res;
        }

        /// <summary>
        /// 返回某设备的点位字符串配置.
        /// </summary>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        public static string CreateDeviceAddressConfigForMegmeet(string deviceid)
        {
            var res = @"{
			    ""DeviceID"": ""(deviceid)"",
			    ""AddressItemList"": [
				    {
					    ""Id"": ""(id1)"",
					    ""Address"": ""SetCurrent"",
					    ""Explain"": ""设定焊接电流"",
					    ""Name"": ""SetCurrent"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""UDWORD"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id2)"",
					    ""Address"": ""SetVoltage"",
					    ""Explain"": ""设定焊接电压"",
					    ""Name"": ""SetVoltage"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""DOUBLE"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id3)"",
					    ""Address"": ""Current"",
					    ""Explain"": ""焊接电流"",
					    ""Name"": ""Current"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""UDWORD"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id4)"",
					    ""Address"": ""Voltage"",
					    ""Explain"": ""焊接电压"",
					    ""Name"": ""Voltage"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""DOUBLE"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id5)"",
					    ""Address"": ""WireSpeed"",
					    ""Explain"": ""送丝速度"",
					    ""Name"": ""WireSpeed"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""DOUBLE"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id6)"",
					    ""Address"": ""Alarm"",
					    ""Explain"": ""报警状态"",
					    ""Name"": ""Alarm"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""INT"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id7)"",
					    ""Address"": ""AlarmMsg"",
					    ""Explain"": ""报警信息"",
					    ""Name"": ""AlarmMsg"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""STRING"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    },
				    {
					    ""Id"": ""(id8)"",
					    ""Address"": ""ConnectStatus"",
					    ""Explain"": ""在线状态"",
					    ""Name"": ""ConnectStatus"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""Read"",
					    ""ValueType"": ""INT"",
					    ""Precision"": ""3"",
					    ""PrecisionType"": ""None"",
					    ""CollectionType"": ""LowFrequency""
				    }
			    ]
		    }";
            res = res.Replace("(deviceid)", deviceid)
                .Replace("(id1)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id2)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id3)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id4)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id5)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id6)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id7)", Guid.NewGuid().ToString().Replace("-", ""))
                .Replace("(id8)", Guid.NewGuid().ToString().Replace("-", ""));
            return res;
        }

        #endregion


        #region OpcDa Test

        public static void Create6DeviceForOpcDa()
        {
            using (var sw1 = new StreamWriter(@"C:\Users\Ryu\Desktop\400devices.cfg", false))
            using (var sw2 = new StreamWriter(@"C:\Users\Ryu\Desktop\400Address.txt", false))
            {
                for (int i = 0; i < 6; i++)
                {
                    var no = (i + 1).ToString().PadLeft(3, '0');
                    var channelid = Guid.NewGuid().ToString().Replace("-", "");
                    var ip = "192.168.0.19";
                    var deviceid = Guid.NewGuid().ToString().Replace("-", "");
                    var devicename = $"opcda_remote_test_19_{no}";
                    sw1.Write(CreateSingleDeviceChannelForOpcDa(channelid, ip, deviceid, devicename));
                    sw1.WriteLine(",");
                    sw2.Write(CreateDeviceAddressConfigForOpcDa(deviceid, $"设备 {i + 1}", i < 5 ? 2000 : 30000));
                    sw2.WriteLine(",");
                }
            }
        }

        /// <summary>
        /// 返回一个单设备通道的字符串配置.
        /// </summary>
        /// <param name="channelid"></param>
        /// <param name="maccode"></param>
        /// <param name="deviceid"></param>
        /// <param name="devicename"></param>
        /// <returns></returns>
        public static string CreateSingleDeviceChannelForOpcDa(string channelid, string ip, string deviceid, string devicename)
        {
            var res = @"{
			    ""ChannelName"": ""(channelid)"",
			    ""ChannelType"": ""OPCDAChannel"",
			    ""ConnectParamList"": [
				    {
					    ""ParaItemName"": ""Ip"",
					    ""ParaItemValue"": ""(ip)""
				    },
				    {
					    ""ParaItemName"": ""ProgID"",
					    ""ParaItemValue"": ""Kepware.KEPServerEX.V6""
				    }
			    ],
			    ""DeviceList"": [
				    {
					    ""DeviceParamList"": [],
					    ""DeviceType"": ""OPCDADevice"",
					    ""DeviceName"": ""(devicename)"",
					    ""DeviceID"": ""(deviceid)"",
					    ""ConfigTime"": 1665455127884,
					    ""Explain"": """",
                        ""StationNumber"": ""1"",
					    ""CompanyName"": ""OPC Foundation"",
					    ""Model"": ""OPC"",
					    ""ProtocolType"": ""OPCDAClient"",
					    ""TimeParam"": {
                    ""ReconDelay"": ""3000"",
						    ""SendMsgDelay"": ""0"",
						    ""ConTimeout"": ""3000"",
						    ""ScanIntervalTime"": ""1000""

                        },
					    ""DeviceCode"": ""(devicename)"",
					    ""ChildCompanyName"": ""(devicename)"",
					    ""Factory"": ""(devicename)"",
					    ""WorkCenter"": ""(devicename)"",
					    ""WorkGroup"": ""(devicename)"",
					    ""Status"": ""Active"",
					    ""CustomerParamList"": [],
					    ""TransmitStatus"": ""Enable"",
					    ""CollectionType"": ""LowFrequency"",
					    ""IsOpenDc"": true,
					    ""IsOpenDnc"": true

                    }
			    ]
		    }";
            res = res.Replace("(channelid)", channelid)
                     .Replace("(ip)", ip)
                     .Replace("(deviceid)", deviceid)
                     .Replace("(devicename)", devicename);
            return res;
        }

        /// <summary>
        /// 返回某设备的点位字符串配置.
        /// </summary>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        public static string CreateDeviceAddressConfigForOpcDa(string deviceid, string opcdevice, int count)
        {
            var res = @"{
			    ""DeviceID"": ""(deviceid)"",
			    ""AddressItemList"": [
                    (addrs)
			    ]
		    }";
            var res2 = @"				    
                    {
					    ""Address"": ""通道 1.(device).(tag)"",
					    ""Name"": ""(name)"",
					    ""ConfigTime"": 1665455166986,
					    ""Authority"": ""ALL"",
					    ""ValueType"": ""DOUBLE"",
					    ""TransformType"": ""None"",
					    ""Express"": ""966"",
					    ""Express"": ""966""
				    },";

            var addrs = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                var t = res2.Replace("(device)", opcdevice)
                   .Replace("(tag)", $"标记 {i.ToString().PadLeft(5, '0')}")
                   .Replace("(name)", $"tag {i.ToString().PadLeft(5, '0')}");
                addrs.Append(t);
            }
            res = res.Replace("(deviceid)", deviceid)
                .Replace("(addrs)", addrs.ToString());
            return res;
        }

        #endregion


        /// <summary>
        /// 生成一个AddressManage配置文件.
        /// </summary>
        private static void CreateADFile()
        {
            // 会覆盖掉C盘正在用的AddressManager.cfg, 所以使用前请备份自己正在用的AM文件.
            using (StreamWriter sw = new StreamWriter(@"C:\RootDC\Project\AddressManage.cfg.", false, Encoding.UTF8))
            {
                sw.WriteLine("{");
                sw.WriteLine("\"AddressManageConfig\" : [");

                AddDevice(sw, "通道 1", "设备 1", "12b576ff44d44dfea98b7e7dbfeb86d0", 2000);
                AddDevice(sw, "通道 1", "设备 2", "12b576ff44d44dfea98b7e7dbfeb86d1", 2000);
                AddDevice(sw, "通道 1", "设备 3", "12b576ff44d44dfea98b7e7dbfeb86d2", 2000);
                AddDevice(sw, "通道 1", "设备 4", "12b576ff44d44dfea98b7e7dbfeb86d3", 2000);
                AddDevice(sw, "通道 1", "设备 5", "12b576ff44d44dfea98b7e7dbfeb86d4", 2000);
                AddDevice(sw, "通道 1", "设备 6", "12b576ff44d44dfea98b7e7dbfeb86d5", 20000);
                AddDevice(sw, "通道 1", "设备 1", "12b576ff44d44dfea98b7e7dbfeb86e0", 2000);
                AddDevice(sw, "通道 1", "设备 2", "12b576ff44d44dfea98b7e7dbfeb86e1", 2000);
                AddDevice(sw, "通道 1", "设备 3", "12b576ff44d44dfea98b7e7dbfeb86e2", 2000);
                AddDevice(sw, "通道 1", "设备 4", "12b576ff44d44dfea98b7e7dbfeb86e3", 2000);
                AddDevice(sw, "通道 1", "设备 5", "12b576ff44d44dfea98b7e7dbfeb86e4", 2000);
                AddDevice(sw, "通道 1", "设备 6", "12b576ff44d44dfea98b7e7dbfeb86e5", 20000);

                sw.WriteLine("]");
                sw.WriteLine("}");
            }
        }

        /// <summary>
        /// 在配置文件内增加一个设备配置.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="chName"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceId"></param>
        /// <param name="itemNum"></param>
        private static void AddDevice(StreamWriter sw, string chName, string deviceName, string deviceId, int itemNum)
        {
            sw.WriteLine("{");
            sw.WriteLine($"\"DeviceID\" : \"{deviceId}\",");
            sw.WriteLine("\"AddressItemList\" : [");

            var tag = $"{chName}.{deviceName}.";
            for (int i = 0; i < itemNum; i++)
            {
                var name = $"标记 {(i + 1).ToString().PadLeft(5, '0')}";
                var content = "{\r\n";
                content += $"\"Address\" : \"{tag + name}\",\r\n";
                content += $"\"Name\" : \"{name}\",";
                content += $"\"ConfigTime\" : \"1638262493785\",\r\n";
                content += $"\"Authority\" : \"ALL\",\r\n";
                content += $"\"ValueType\" : \"DOUBLE\",\r\n";
                content += $"\"TransformType\" : \"None\",\r\n";
                content += $"\"Express\" : \"966\",\r\n";
                content += "},\r\n";

                sw.Write(content);
            }

            sw.WriteLine("]");
            sw.WriteLine("},");
        }

        /// <summary>
        /// 将opc browser的点位浏览输出为树形图.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="prefixSpace"></param>
        /// <param name="overflowLimit"></param>
        /// <param name="expandLimit"></param>
        /// <param name=""></param>
        private static void OutputTree(OPCBrowser b, int prefixSpace, int overflowLimit, int expandLimit)
        {
            int i = 0;
            var error = false;
            try
            {
                b.ShowBranches();
            }
            catch (Exception)
            {
                error = true;
            }

            if (!error)
            {
                foreach (var item in b)
                {
                    i++;
                    Console.WriteLine("".PadLeft(prefixSpace, ' ') + "|__" + "(分支)" + item);

                    try
                    {
                        b.MoveDown(item.ToString());
                        OutputTree(b, prefixSpace + 4, overflowLimit, expandLimit);
                        b.MoveUp();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            try
            {
                b.ShowLeafs(false);
            }
            catch (Exception)
            {
                error = true;
            }

            if (!error)
            {
                var x = b.Count;

                if (x > overflowLimit)
                {
                    Console.WriteLine("".PadLeft(prefixSpace, ' ') + "|__" + "叶子节点数目溢出，已忽略显示...");
                    return;
                }

                foreach (var item in b)
                {
                    i++;
                    Console.WriteLine("".PadLeft(prefixSpace, ' ') + "|__" + item + "  ID:" + b.GetItemID(item.ToString())+"  AR:"+b.AccessRights);

                    if (i > expandLimit)
                    {
                        Console.WriteLine("".PadLeft(prefixSpace, ' ') + "|__" + "超过叶子节点展开上限，停止展开.");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// opcda数据类型测试.
        /// </summary>
        /// <param name="args"></param>
        private static void OpcDataTypeTest(string[] args)
        {
            try
            {
                // test
                string ip1 = "10.16.134.200";
                var x1 = new OPCServer();
                var y1 = x1.GetOPCServers(ip1);


                // 读入参数
                string ip = args.Length > 0 ? args[0] : "127.0.0.1";
                string progID = args.Length > 1 ? args[1] : "Kepware.KEPServerEX.V6";
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
        }

        /*
        /// <summary>
        /// 填充opc树节点.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="node"></param>
        /// <param name="id"></param>
        private static void FillOpcNode(OPCBrowser browser, OpcDaItem node, ref int id)
        {
            // 错误标记
            var error = false;

            try
            {
                // 展开分支节点
                browser.ShowBranches();
            }
            catch (Exception ex)
            {
                IOLog.Error($"[OpcDaDebug - OutputTree] show branched failed.\r\n{ex}");
                error = true;
            }

            // 若浏览分支正常，则构造分支
            if (!error)
            {
                foreach (var item in browser)
                {
                    var branch = node.AddItem(id++, item.ToString(), browser.GetItemID(item.ToString()));

                    try
                    {
                        browser.MoveDown(item.ToString());
                        FillOpcNode(browser, branch, ref id);
                        browser.MoveUp();
                    }
                    catch (Exception ex)
                    {
                        IOLog.Error($"[OpcDaDebug - OutputTree] move down, move up or fill a branch failed.\r\n{ex}");
                    }
                }
            }

            error = false;
            try
            {
                // 展开当前层级的tag节点
                browser.ShowLeafs(false);
            }
            catch (Exception ex)
            {
                IOLog.Error($"[OpcDaDebug - OutputTree] show leafs failed.\r\n{ex}");
                error = true;
            }

            if (!error)
            {
                foreach (var item in browser)
                {
                    node.AddItem(id++, item.ToString(), browser.GetItemID(item.ToString()), true);
                }
            }
        }
        */
    }
}

