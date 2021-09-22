using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System.Globalization;
using System.Collections.Generic;

namespace SalesWebMvc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // alterado na aula 249 - 5:00 p/ conectar com mysql
            // requer PROVIDER p/ mysql
            services.AddDbContext<SalesWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                        builder.MigrationsAssembly("SalesWebMvc")));

            // aula 251 - injeta dependencia para popular base de dados com dados iniciais
            services.AddScoped<SeedingService>();

            //aula 254
            services.AddScoped<SellerService>();

            //aula 257
            services.AddScoped<DepartmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // 
        // metodo alterado na aula 252 - injeção do SeedingService
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {

            // aula 262 - configs de localidade
            var enUS = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUS),
                SupportedCultures = new List<CultureInfo> { enUS },
                SupportedUICultures = new List<CultureInfo> { enUS }
            };
            app.UseRequestLocalization(localizationOptions);

            // verifica se está no ambiente de desenvolvimento
            // ou no ambiente de produção (app publicado)
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // popula base de dados
                seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
