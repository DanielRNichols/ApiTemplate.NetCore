using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ApiTemplate.NetCore.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using ApiTemplate.NetCore.Services;
using ApiTemplate.NetCore.Interfaces;
using AutoMapper;
using ApiTemplate.NetCore.Mappings;
using BookStoreApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiTemplate.NetCore
{
    public class Startup
    {
        private readonly string CorsPolicyName = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()  //options => options.SignIn.RequireConfirmedAccount = true)
                // Add Roles service
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // AutoMapper
            services.AddAutoMapper(typeof(Maps));

            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:issuer"],
                        ValidAudience = Configuration["Jwt:issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))

                    };

                });



            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // SwaggerGen Swashbuckle
            services.AddSwaggerGen(cfg =>  
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "Api",  
                    Version = "v1",
                    Description = "Api Template"
                });

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

                cfg.IncludeXmlComments(xmlFullPath);

            });

            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IItemRepository, ItemRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Seed the database
            SeedData.Seed(userManager, roleManager).Wait();

            app.UseCors(CorsPolicyName);

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(cfg => 
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
                cfg.RoutePrefix = ""; ;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
