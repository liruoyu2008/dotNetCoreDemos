using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SpectreConsoleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            YouDaoApiService.GetTranslatioAsync("take", "en", "zh").Wait();
        }
    }
}
