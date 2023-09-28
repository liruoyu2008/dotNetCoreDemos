using System;
using System.Linq;

namespace HttpServerDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = args.FirstOrDefault() ?? "8080";
            var port = int.TryParse(str, out int p) ? p : 8080;
            WebAPIStart.StartWebAPI(port);

            Console.ReadLine();
        }
    }
}