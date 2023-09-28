using System;
using Sygole.R2KReader;

namespace SygoleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reader = new R2KReader();
            reader.Connect("192.168.1.20", 3001);
        }
    }
}
