using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.DAL.Impl;
using FilmSearch.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FilmSearch.Models;
using FilmSearch.Services;

namespace FilmSearch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureDal(IServiceCollection services)
        {
            services.AddDbContext<FilmSearchContext>(
                options => options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("FilmSearch"))
            );
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<FilmService>();
            services.AddScoped<PersonService>();
            
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<FilmSearchContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
            
            ConfigureDal(services);

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Auth/LogIn");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
            }


            app.UseBrowserLink();
   //         app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
