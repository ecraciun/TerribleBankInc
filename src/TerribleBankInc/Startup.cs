using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TerribleBankInc.Data;
using TerribleBankInc.Repositories;
using TerribleBankInc.Services;
using AutoMapper;
using System.Reflection;
using Microsoft.OpenApi.Models;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;
using ElmahCore.Mvc;
using ElmahCore;
using TerribleBankInc.Filters;

namespace TerribleBankInc
{
    public class Startup
    {
        public const string TerribleCookieSchemeName = "TerribleIncScheme";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.LogPath = "~/log";
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddDbContext<TerribleBankDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options =>
            {
                options.Filters.Add<CustomExceptionFilterAttribute>();
            });

            services.AddAuthentication(TerribleCookieSchemeName)
                .AddCookie(TerribleCookieSchemeName, options =>
                {
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.ClaimsIssuer = "TerribleBankInc";
                });
            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankTransferService, BankTransferService>();
            services.AddScoped<IHashingService, SimpleHashingService>();
            //services.AddScoped<IHashingService, BetterHashingService>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "TerribleBank API", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TerribleBankDbContext>();
                context?.Database.Migrate();
            }

            app.UseStaticFiles();

            app.UseElmah();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "api",
                //    pattern: "api/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
