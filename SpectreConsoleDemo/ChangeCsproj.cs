using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectreConsoleDemo
{
    public class ChangeCsproj
    {
        public static void Test()
        {
            var path1 = @"D:\code\origin\dc-master\Main";

            var dirs = Directory.GetDirectories(path1);
            foreach (var typeDir in dirs)
            {
                var projDirs = Directory.GetDirectories(typeDir);
                foreach (var proj in projDirs)
                {
                    var files = Directory.GetFiles(proj, "*.csproj");
                    foreach (var file in files)
                    {
                        string content = null;
                        byte[] bytes = File.ReadAllBytes(file);
                        using (StreamReader sr = new StreamReader(file, true))
                        {
                            content = sr.ReadToEnd();
                        }
                        using (StreamWriter sw = new StreamWriter(file, false, new UTF8Encoding(isBomHeader(bytes))))
                        {
                            var newContent = content.Replace(@"<OutputPath>..\bin\</OutputPath>", @"<OutputPath>..\..\bin\</OutputPath>");
                            sw.Write(newContent);
                        }
                    }
                }
            }
        }

        public static bool isBomHeader(byte[] bs)
        {
            int len = bs.Length;
            if (len >= 3 && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
            {
                return true;
            }
            return false;
        }
    }
}
