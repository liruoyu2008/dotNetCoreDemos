using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

namespace SerilogDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            // 将配置传给 Serilog 的提供程序 
                            //.ReadFrom.Configuration(Configuration)
                            .Enrich.With(new DateTimeNowEnricher())
                            .MinimumLevel.Debug()//最小记录级别
                            .Enrich.FromLogContext()//记录相关上下文信息 
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)//对其他日志进行重写,除此之外,目前框架只有微软自带的日志组件
                                                                                          //.WriteTo.Console()//输出到控制台 //.WriteTo.File
                            .WriteTo.Async(o =>
                            {
                                o.Console();
                                o.File("logs/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/log_.log", restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day); //输出到文件,需要提供输出路径和周期
                            })
                            .CreateLogger();



            Log.Information("Starting web host");
            Log.Error("Starting web host failed!");
            Console.ReadLine();
        }


        #region Serilog 相关设置
        class DateTimeNowEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "DateTimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion
    }
}
