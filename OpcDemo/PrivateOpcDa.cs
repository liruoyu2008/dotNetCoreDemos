using OPCAutomation;
using RootLink.DC.Common;
using RootLink.DC.TraceLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpcDemo
{
    internal class PrivateOpcDa
    {
        /*

        static string kep_server = "Kepware.KEPServerEX.V6";
        static string supcon_server = "SUPCON.JXServer.1";
        static string ip = "127.0.0.1";
        static string kep_item = "通道 1.设备 1.标记 00001";
        static string supcon_item = "fjys_A";

        static int globalid = 0;

        public static void Main2(string[] args)
        {
            var sb = MakeTreeToStringBuilder(0);
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Ryu\Desktop\server2client.txt", false, Encoding.UTF8))
            {
                sw.WriteLine(sb);
            }

            var tree = MakeStringBuilderToTree(sb, 0);

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        /// <summary>
        /// 将opc点位结构转换为字符串，方便网络传输.
        /// </summary>
        /// <param name="rootid"></param>
        /// <returns></returns>
        private static StringBuilder MakeTreeToStringBuilder(int rootid)
        {
            var server = new OPCServer();
            server.Connect(supcon_server, ip);
            var b = server.CreateBrowser();
            var sb = new StringBuilder();
            PlatAOpcNode(b, sb, globalid, '^', '|');
            return sb;
        }

        /// <summary>
        /// 将一个opcda节点拍扁成字符串.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="sb"></param>
        /// <param name="parentId">父节点id.</param>
        /// <param name="insideSeperator">item内属性分隔符.</param>
        /// <param name="outsideSeperator">item间分隔符.</param>
        private static void PlatAOpcNode(OPCBrowser browser, StringBuilder sb, int parentId, char insideSeperator, char outsideSeperator)
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
                    sb.Append(
                        // 此处每行数据都是opc item的一个属性，酌情选择需要传输的数据即可。仅影响网络传输的数据量。
                        $"{item}{insideSeperator}" +    // tag name 用于展示
                        $"{browser.GetItemID(item.ToString())}{insideSeperator}" +  // tag address 用于订阅数据
                        $"{++globalid}{insideSeperator}" +    // global unique id 用于还原树结构
                        $"{parentId}{insideSeperator}" +    // parent id 用于还原树结构
                        $"{browser.DataType}{insideSeperator}" +    // 数据类型
                        $"0{outsideSeperator}"  // 0为非opc tag，1为opc tag
                        );

                    try
                    {
                        browser.MoveDown(item.ToString());
                        PlatAOpcNode(browser, sb, globalid, insideSeperator, outsideSeperator);
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
                    sb.Append(
                        $"{item}{insideSeperator}" +
                        $"{browser.GetItemID(item.ToString())}{insideSeperator}" +
                        $"{++globalid}{insideSeperator}" +
                        $"{parentId}{insideSeperator}" +
                        $"{browser.DataType}{insideSeperator}" +
                        $"1{outsideSeperator}"
                        );
                }
            }
        }

        /// <summary>
        /// 使用字符串构造一颗opcda节点树.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rootid">指定的根节点id. 不能与字符串内已有的节点id相同. 建议使用0或任意负数</param>
        /// <returns></returns>
        private static OpcDaItem MakeStringBuilderToTree(StringBuilder sb, int rootid)
        {
            var root = new OpcDaItem(rootid, "root", "root");
            root.Children = new List<OpcDaItem>();

            var res = new List<OpcDaItem>();
            var dic = new Dictionary<int, OpcDaItem>();
            foreach (var item in sb.ToString().Split('|', StringSplitOptions.RemoveEmptyEntries))
            {
                var props = item.Split('^', StringSplitOptions.None);

                var node = new OpcDaItem(int.Parse(props[2]), props[0], props[1], props[5] == "1");
                node.ParentID = int.Parse(props[3]);
                node.DataType = props[4];

                dic.Add(node.ID, node);
                if (dic.TryGetValue(node.ParentID, out OpcDaItem parent))
                {
                    if (parent.Children == null)
                    {
                        parent.Children = new List<OpcDaItem>();
                    }
                    parent.Children.Add(node);
                    parent.HasChild = true;
                }
                else
                {
                    root.Children.Add(node);
                    root.HasChild = true;
                }
            }

            return root;
        }

        */
    }
}
