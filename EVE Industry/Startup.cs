using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EVE_Industry.EfStuff;
using EVE_Industry.EfStuff.Repositories;
using EVE_Industry.Services;
using Microsoft.AspNetCore.Http;

namespace EVE_Industry
{
    public class Startup
    {
        public const string AuthCoockieName = "Smile";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //var connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EveIndustry;Integrated Security=True;";
            var connectString = @"Server=EveIndustry.mssql.somee.com;Database=EveIndustry;User Id=Kadabra_SQLLogin_2;Password=7tf3w9f93c;";

            services.AddDbContext<WebContext>(x => x.UseSqlServer(connectString));


            services.AddAuthentication(AuthCoockieName)
                .AddCookie(AuthCoockieName, config =>
                {
                    config.LoginPath = "/User/Login";
                    config.AccessDeniedPath = "/User/AccessDenied";
                    config.Cookie.Name = "AuthSweet";
                });


            services.AddScoped<MainIndustryRepository>();
            services.AddScoped<DumpRepository>();

            RegisterMapper(services);


            services.AddScoped<DumpService>();

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
        }
        private void RegisterMapper(IServiceCollection services)
        {
            var provider = new MapperConfigurationExpression();

            //provider.CreateMap<User, UserViewModel>();
            //provider.CreateMap<UserViewModel, User>();



            var mapperConfiguration = new MapperConfiguration(provider);

            var mapper = new Mapper(mapperConfiguration);

            services.AddScoped<IMapper>(x => mapper);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
