using DocsStorageWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocsStorageWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDdContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                     .SetIsOriginAllowed(e => true)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IDocRepository, DocumentsRepository>();
            services.AddSingleton(new StorageSettings() { PathToFolder = Configuration["StorageSettings:PathToFolder"] });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
      
            app.UseMvc();

            using (var serviseScoped = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviseScoped.ServiceProvider.GetRequiredService<AppDdContext>().Database.Migrate();
            }
        }
    }
}
