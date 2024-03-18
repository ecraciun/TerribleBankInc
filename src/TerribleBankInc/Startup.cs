using System.Reflection;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TerribleBankInc.Data;
using TerribleBankInc.Filters;
using TerribleBankInc.Repositories;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc;

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

        services.AddDbContext<TerribleBankDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddMvc(options =>
        {
            options.Filters.Add<CustomExceptionFilterAttribute>();
        });

        services
            .AddAuthentication(TerribleCookieSchemeName)
            .AddCookie(
                TerribleCookieSchemeName,
                options =>
                {
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.ClaimsIssuer = "TerribleBankInc";
                }
            );
        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<IBankTransferService, BankTransferService>();
        services.AddScoped<IHashingService, SimpleHashingService>();
        //services.AddScoped<IHashingService, BetterHashingService>();

        services.AddDatabaseDeveloperPageExceptionFilter();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TerribleBank API", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using (
            IServiceScope serviceScope = app
                .ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope()
        )
        {
            TerribleBankDbContext context =
                serviceScope.ServiceProvider.GetService<TerribleBankDbContext>();
            context?.Database.Migrate();
        }

        app.Use(
            (context, next) =>
            {
                context.Response.Cookies.Append(
                    "Leaky",
                    "Very sensitive data",
                    new CookieOptions() { }
                );

                #region Later

                //context.Response.Headers.Add("Referrer-Policy", "same-origin");

                //context.Response.Headers.Add("Feature-Policy", "geolocation 'none';");

                //context.Response.Headers.Add("Referrer-Policy", "same-origin");

                //context.Response.Headers.Add("X-Frame-Options", "DENY");

                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-inline';");

                #endregion

                return next();
            }
        );

        app.UseStaticFiles();

        app.UseElmah();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseDeveloperExceptionPage();

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
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
}
