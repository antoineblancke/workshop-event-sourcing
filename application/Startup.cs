using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using domain.account;
using domain.common;
using infrastructure.infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using projection;

namespace application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IBalanceRepository, InMemoryBalanceRepository>();
            services.AddTransient<IProjectionManager, BalanceProjectionManager>();
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddTransient<IProcessManager, TransferProcessManager>();
            services.AddSingleton<IEventBus, InMemoryEventBus>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            eventBus.Register(app.ApplicationServices.GetService<IProcessManager>());
            eventBus.Register(app.ApplicationServices.GetService<IProjectionManager>());
        }
    }
}
