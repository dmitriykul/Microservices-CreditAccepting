using System;
using System.IO;
using System.Reflection;
using Application_acceptance_service.App;
using Application_acceptance_service.App.Types;
using Application_acceptance_service.Infrastructure;
using Application_acceptance_service.Infrastructure.Automapper;
using Application_acceptance_service.Infrastructure.Database;
using Application_acceptance_service.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application_acceptance_service
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
            
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            try
            {
                var options = optionsBuilder
                    //.UseLazyLoadingProxies()
                    .UseNpgsql(connection)
                    .Options;
                using var db = new ApplicationContext(options);
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                return;
            }
            
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<ApplicationAcceptanceOptions>(Configuration.GetSection("ApplicationAcceptanceOptions"));
            services.AddAutoMapper(typeof(AppMappingProfile));
            services.AddTransient<IRepository<RequestedCreditDto>, RequestedCreditRepository>();
            services.AddTransient<IRepository<ApplicantDto>, ApplicantRepository>();
            services.AddTransient<IRepository<ApplicationDto>, ApplicationRepository>();
            services.AddTransient<ApplicationManager>();
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}