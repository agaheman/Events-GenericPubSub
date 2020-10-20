using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericPubSubLibrary;
using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PublisherApp
{
    public class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.RabbitMq.json", optional: false, reloadOnChange: true)
                .Build();

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<RabbitOptions>(Configuration.GetSection("RabbitMqConnection"));

            var rabbitOptions = new RabbitOptions();
            Configuration.GetSection("RabbitMqConnection").Bind(rabbitOptions);

            services.AddSingleton<IRabbitPublisher, DefaultPublisherService>(serviceProvider =>
            {
                return new DefaultPublisherService(rabbitOptions);
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
