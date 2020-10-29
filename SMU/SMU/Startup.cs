using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMU.Models;

namespace SMU
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddScoped<IRequestManager, RequestManager>();
            services.AddScoped<IClaimManager, ClaimManager>();
            services.AddScoped<ISeedData, SeedData>();

            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(
                    Configuration.GetConnectionString("PostgreSQLConnection")
                    )
                );
            
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });

            services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireNonAlphanumeric = false;
                //option.SignIn.RequireConfirmedEmail = true;
            }
            ).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedData seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            seed.Seed();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });                       
        }
    }
}
