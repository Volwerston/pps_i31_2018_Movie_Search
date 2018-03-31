using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FilmSearch.Models;
using FilmSearch.DAL;
using FilmSearch.DAL.Impl;

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

            services
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IPersonRepository, PersonRepository>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<FilmSearchContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();
            
            ConfigureDal(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
