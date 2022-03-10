using OpenProtocolInterpreter;
using System;

namespace MIDInterpreterDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mider = new MidInterpreter()
                .UseCommunicationMessages()
                .UseParameterSetMessages()
                .UseToolMessages()
                .UseTighteningMessages()
                .UseAlarmMessages();
            var str0061 = @"02310061001000000000010011020003IR Insight               04No BCode                 050006001070000080000090100111120416001306240014069500150005651600000173200018000001900963202022-02-24:13:03:5721Last Change Unknown22223130800    .";
            var x = mider.Parse(str0061);
            var str0002 = @"00570002001         010000020003姗¤兌鎮灦-Left        ";
            x = mider.Parse(str0002);
            var str0065 = @"02260065002         01000013244202                         03000004001050106000000700100800100911011111211311411511611711800000000001905824520000222100000220002300000024000000250000026000002719E74301      282022-03-08:15:49:15";
            x = mider.Parse(str0065);
            var str0013 = @"01040013001         0100302鍙嶈浆                   0320400050000000606600007000000080010009004001000300";
            x = mider.Parse(str0013);
            var str0041 = @"00810041001         0119E74301      020000441816032020-07-28:00:00:00046272001734";
            x = mider.Parse(str0041);
            var str0076 = @"00560076001         01002E000031041051970-01-01:00:00:00";
            x = mider.Parse(str0076);
            var str0071 = @"00530071001 01      01I016020030042022-03-10:14:44:40";
            x = mider.Parse(str0071);
            var str0078 = @"00200078            ";
            x = mider.Parse(str0078);
            Console.ReadKey();
        }
    }
}
