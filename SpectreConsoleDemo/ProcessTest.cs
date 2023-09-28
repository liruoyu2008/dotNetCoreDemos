using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SpectreConsoleDemo
{
    internal class ProcessTest
    {
        OpcdaProcessInfo _process = null;

        void Test()
        {
            try
            {
                _process = Start();
                Thread.Sleep(10);
                if (_process.Process.HasExited)
                {
                    int a = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("按Q结束进程并退出.");

            while (true)
            {
                var x = Console.ReadKey();
                if (x.Key == ConsoleKey.Q)
                {
                    Stop(_process.Process);
                    return;
                }
            }
        }

        /// <summary>
        /// 启动opcda调试
        /// </summary>
        /// <returns></returns>
        public OpcdaProcessInfo Start()
        {
            var res = new OpcdaProcessInfo();

            var port = 34567;
            var processStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RootLink.DC.Protocol.OpcDaDebug.exe"),
                Arguments = $"{port}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var p = new Process();
            p.StartInfo = processStartInfo;
            p.Exited += Process_Exited;

            res.Process = p;
            res.Port = port;

            p.Start();
            return res;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            var p = sender as Process; if (p != null)
            {
                Console.WriteLine($"输出：{p.StandardOutput.ReadToEnd()}");
                Console.WriteLine($"错误：{p.StandardError.ReadToEnd()}");
            }
        }

        /// <summary>
        /// 停止进程
        /// </summary>
        /// <param name="process"></param>
        public void Stop(Process process)
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        }

    }

    /// <summary>
    /// pocda进程信息
    /// </summary>
    public class OpcdaProcessInfo
    {
        public Process Process { get; set; }

        public StreamReader ErrorStream { get; set; }

        public StreamReader OutputStream { get; set; }

        public int Port { get; set; }
    }
}
