using Opc;
using Opc.Da;

var cd1= new ConnectData(new System.Net.NetworkCredential("Administrator", "a6w/tT8_VzX+J"));
var cd2 = new ConnectData(new System.Net.NetworkCredential("ryu", "Li@12345"));
var serverEnumerator = new OpcCom.ServerEnumerator();
var servers = serverEnumerator.GetAvailableServers(Specification.COM_DA_20, "10.70.37.12",cd1);
foreach (var item in servers)
{
    Console.WriteLine("Server Name: " + item.Name);
    Console.WriteLine("Server URL: " + item.Url);
}

var server = new Opc.Da.Server(new OpcCom.Factory(true), new Opc.URL("opcda://10.70.37.12/kepware.KEPServerEX.V6"));
server.Connect(cd1);

var server1 = new Opc.Hda.Server(new OpcCom.Factory(true), new Opc.URL("opchda://10.70.37.12/kepware.KEPServerEX.V6"));
server1.Connect(cd1);
var b =  server1.CreateBrowser(null,out var x).Browse(null);
foreach (var item in b)
{
    Console.WriteLine(b);
}

// 浏览根项
var root = new Opc.Dx.ItemIdentifier(null, null);
var filter = new BrowseFilters() { ReturnPropertyValues = true };

// 获取根项下的所有子项
var itemIdentifiers = server.Browse(root, filter, out BrowsePosition position);

// 遍历获取到的可用项
foreach (var item in itemIdentifiers)
{
    if (item.HasChildren)
    {
        var xxx = server.Browse(new Opc.Dx.ItemIdentifier(item.ItemName), filter, out position);
    }
    Console.WriteLine("Item: " + item.ItemName);
}

// 设置要读取的项的路径（替换为您实际的路径）
var itemName = "数据类型示例.8 位设备.K 寄存器.ByteArray";

var props = server.GetProperties(new Opc.ItemIdentifier[] { new Opc.ItemIdentifier(itemName) }, new PropertyID[] {
    Property.DATATYPE,Property.DESCRIPTION,Property.ACCESSRIGHTS
}, true);

// 创建读取请求
var items = new Item[] { new Item { ItemName = itemName } };
var results = server.Read(items);

// 输出读取到的值
foreach (var result in results)
{
    Console.WriteLine("Item: " + result.ItemName);
    Console.WriteLine("Value: " + result.Value);
}


Console.ReadLine();
