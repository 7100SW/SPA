using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace SPA
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
            services.AddSpaStaticFiles();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SPA", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPA v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });

            app.Map("/guests", mappedSpa =>
            {
                mappedSpa.UseSpa(spa =>
                {
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                    {
                        FileProvider =
                            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/guests"))
                    };
                    spa.Options.SourcePath = "wwwroot/guests";
                });
            });

            app.Map("/members", mappedSpa =>
            {
                mappedSpa.UseSpa(spa =>
                {
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                    {
                        FileProvider =
                            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/members"))
                    };
                    spa.Options.SourcePath = "wwwroot/members";
                });
            });

        }
    }
}
