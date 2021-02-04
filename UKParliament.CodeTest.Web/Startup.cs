using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Web
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
            //services.AddTransient<IRoomBookingsRepository, DbRepository>();
            services.AddControllers();
            services.AddDbContext<RoomBookingsContext>(op => op.UseInMemoryDatabase("RoomBookings"));
            services.AddMvc();

            services.AddSwaggerGen();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assembles = new List<Assembly>()
            {
                typeof(RoomBookingsContext).Assembly,
                typeof(IPersonService).Assembly
            };

            foreach (var assembly in assembles)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .AsImplementedInterfaces();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
