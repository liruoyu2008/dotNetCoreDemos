using HttpServerDemo.Transformers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HttpServerDemo
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHostedService<LifetimeEventsHostedService>();
            services.AddLogging();
            services.AddCors(options =>
            {
                options.AddPolicy("any",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddControllers(option =>
            {
                option.EnableEndpointRouting = false;
                option.InputFormatters.Add(new RawRequestBodyFormatter());
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            // 使用跨域配置
            app.UseCors("any");
            app.UseMvc();
        }
    }

}
