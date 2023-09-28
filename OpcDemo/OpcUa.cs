using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpcDemo
{

    internal class OpcUa
    {
        // 所有遇到的数据类型数列
        static List<string> _dt = new List<string>();
        static List<string> _dt2 = new List<string>();

        public static void Main2(string[] args)
        {
            var client = new OpcUaClient();
            client.UserIdentity = new UserIdentity("opcua", "opcua");
            client.ConnectServer("opc.tcp://localhost:49320").Wait();

            var n = new NodeId("模拟器示例", 2);
            var root = new OpcUaItem()
            {
                Identifier = ObjectIds.ObjectsFolder.Identifier,
                Namespace = client.Session.NamespaceUris.GetString(ObjectIds.ObjectsFolder.NamespaceIndex)
            };
            //root = new OpcUaItem()
            //{
            //    Identifier = n.Identifier,
            //    Namespace = "KEPServerEX"
            //};

            // test
            root = FillTree3(client, root, true);
            var root2 = FillTree2(client, root, true);

            // 获取一个地址转换NodeId的示例
            var n1 = GetNodeId(client.Session.NamespaceUris, root[0][1].Address);
            var n2 = GetNodeId(client.Session.NamespaceUris, root[4][2][2].Address);

            // 订阅数据
            client.AddSubscription("key", new[] { n1.ToString(), n2.ToString() }, DataChanged);

            Console.ReadKey();
        }

        public static void DataChanged(string str, MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            var temp = e.NotificationValue as MonitoredItemNotification;
            if (temp != null)
            {
                Console.WriteLine($"{item.DisplayName}  :   {temp.Value}");
            }
        }

        /// <summary>
        /// 将item地址转化为NodeId
        /// </summary>
        /// <param name="table"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static NodeId GetNodeId(NamespaceTable table, string address)
        {
            try
            {
                var strs = address.Split('@');
                var id = strs[0];
                var nsi = table.GetIndex(strs[1]);

                if (uint.TryParse(id, out uint idInt))
                {
                    return new NodeId(idInt, (ushort)nsi);

                }
                else if (Guid.TryParse(id, out Guid guid))
                {
                    return new NodeId(guid, (ushort)nsi);
                }
                else
                {
                    return new NodeId(id, (ushort)nsi);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}:Convert address to NodeId failed.");
            }

            return NodeId.Null;
        }

        /// <summary>
        /// 填充节点树的某节点，并返回该节点;可递归.
        /// 默认只搜索对象和变量节点，且所有变量当作叶子节点，不递归搜索二级变量.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uaItem"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static OpcUaItem FillTree(OpcUaClient client, OpcUaItem uaItem, bool recursive)
        {
            // 读取引用
            var nodeToBrowse = new BrowseDescription()
            {
                NodeId = new NodeId(uaItem.Identifier, (ushort)client.Session.NamespaceUris.GetIndex(uaItem.Namespace)),
                BrowseDirection = BrowseDirection.Forward,
                ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                IncludeSubtypes = true,
                NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable),
                ResultMask = (uint)(BrowseResultMask.All)
            };
            var refs = ClientUtils.Browse(client.Session, nodeToBrowse, false);

            if (refs != null)
            {
                var id = 1;
                foreach (var r in refs)
                {
                    var uaItem2 = new OpcUaItem()
                    {
                        Identifier = r.NodeId.Identifier,
                        Namespace = client.Session.NamespaceUris.GetString(r.NodeId.NamespaceIndex),
                        DataType = "OBJECT",
                    };

                    // 读取节点的Attribute(主要耗时语句)
                    var attrs = client.ReadNoteAttributes(r.NodeId.ToString());

                    // 是否Var节点
                    var varAttr = attrs.FirstOrDefault(it => it.Name == "Value");
                    if (varAttr != null)
                    {
                        uaItem2.IsVar = true;

                        // 记住数据类型
                        if (!_dt.Contains(varAttr.Type))
                        {
                            _dt.Add(varAttr.Type);
                        }

                        // 记住数据类型
                        if (!_dt2.Contains(uaItem2.DataType))
                        {
                            _dt2.Add(uaItem2.DataType);
                        }
                    }

                    // id赋值
                    uaItem2.ID = string.IsNullOrWhiteSpace(uaItem.ID)
                        ? (id).ToString()
                        : $"{uaItem.ID}_{id}";

                    // 非Var节点则递归
                    if (uaItem2.IsVar == false && recursive)
                    {
                        uaItem2 = FillTree(client, uaItem2, recursive);
                    }

                    // 排除空的非叶子节点
                    if (uaItem2.Children.Count > 0 || varAttr != null)
                    {
                        uaItem.Children.Add(uaItem2);
                        id++;
                    }
                }
            }

            return uaItem;
        }

        /// <summary>
        /// 填充节点树的某节点，并返回该节点;可递归.
        /// 默认只搜索对象和变量节点，且所有变量当作叶子节点，不递归搜索二级变量.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uaItem"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static OpcUaItem FillTree2(OpcUaClient client, OpcUaItem uaItem, bool recursive)
        {
            // 读取引用
            var children = client.BrowseNodeReference(new NodeId(uaItem.Identifier, (ushort)client.Session.NamespaceUris.GetIndex(uaItem.Namespace)).ToString());

            if (children != null && children.Length > 0)
            {
                var id = 1;
                foreach (var r in children)
                {
                    var uaItem2 = new OpcUaItem()
                    {
                        Identifier = r.NodeId.Identifier,
                        Namespace = client.Session.NamespaceUris.GetString(r.NodeId.NamespaceIndex),
                        DataType = "OBJECT",
                    };

                    // 是否Var节点
                    if (r.NodeClass == NodeClass.Variable)
                    {
                        uaItem2.IsVar = true;
                    }

                    // id赋值
                    uaItem2.ID = string.IsNullOrWhiteSpace(uaItem.ID)
                        ? (id).ToString()
                        : $"{uaItem.ID}_{id}";

                    // 非Var节点则递归
                    if (uaItem2.IsVar == false && recursive)
                    {
                        uaItem2 = FillTree2(client, uaItem2, recursive);
                    }

                    // 排除空的非叶子节点
                    if (uaItem2.Children.Count > 0 || uaItem2.IsVar == true)
                    {
                        uaItem.Children.Add(uaItem2);
                        id++;
                    }
                }
            }

            return uaItem;
        }

        /// <summary>
        /// 填充节点树的某节点，并返回该节点;可递归.
        /// 默认只搜索对象和变量节点，且所有变量当作叶子节点，不递归搜索二级变量.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uaItem"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static OpcUaItem FillTree3(OpcUaClient client, OpcUaItem uaItem, bool recursive)
        {
            // 读取引用
            var nodeToBrowse = new BrowseDescription()
            {
                NodeId = new NodeId(uaItem.Identifier, (ushort)client.Session.NamespaceUris.GetIndex(uaItem.Namespace)),
                BrowseDirection = BrowseDirection.Forward,
                ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                IncludeSubtypes = true,
                NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable),
                ResultMask = (uint)(BrowseResultMask.All)
            };
            var refs = ClientUtils.Browse(client.Session, nodeToBrowse, false);

            if (refs != null)
            {
                var id = 1;
                foreach (var r in refs)
                {
                    var uaItem2 = new OpcUaItem()
                    {
                        Identifier = r.NodeId.Identifier,
                        Namespace = client.Session.NamespaceUris.GetString(r.NodeId.NamespaceIndex),
                        DataType = "OBJECT",
                    };

                    // 是否Var节点
                    if (r.NodeClass ==NodeClass.Variable)
                    {
                        uaItem2.IsVar = true;
                    }

                    // id赋值
                    uaItem2.ID = string.IsNullOrWhiteSpace(uaItem.ID)
                        ? (id).ToString()
                        : $"{uaItem.ID}_{id}";

                    // 非Var节点则递归
                    if (uaItem2.IsVar == false && recursive)
                    {
                        uaItem2 = FillTree3(client, uaItem2, recursive);
                    }

                    // 排除空的非叶子节点
                    if (uaItem2.Children.Count > 0 || uaItem2.IsVar)
                    {
                        uaItem.Children.Add(uaItem2);
                        id++;
                    }
                }
            }

            return uaItem;
        }
    }

    /// <summary>
    /// opcua使用的点位结构
    /// </summary>
    class OpcUaItem
    {
        public OpcUaItem this[int index]
        {
            get { return Children[index]; }
            set { Children[index] = value; }
        }

        /// <summary>
        /// 整棵树的唯一字符串id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 节点对应的服务
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 节点在服务内的唯一标识符
        /// </summary>
        public object Identifier { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<OpcUaItem> Children { get; set; } = new List<OpcUaItem>();

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否为数据变量（数据变量具有数值，可监控）
        /// </summary>
        public bool IsVar { get; set; }

        /// <summary>
        /// 变量地址
        /// </summary>
        public string Address { get { return $"{Identifier}@{Namespace}"; } }

        public override string ToString()
        {
            return Address;
        }
    }
}
