using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectreConsoleDemo
{
    internal class SelfLog
    {
        public void Write(string filename, string msg)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
                if (File.Exists(filename))
                {
                    File.Delete((filename));
                }
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
