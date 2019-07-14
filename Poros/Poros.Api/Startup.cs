using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poros.Api.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace Poros.Api
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "https://localhost:44357";
                o.Audience = "resourceapi";
                o.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.read"));
                options.AddPolicy("Passenger", policy => policy.RequireClaim(ClaimTypes.Role, "passenger"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Poros API",
                    Description = "APIs for Poros Platform",
                    TermsOfService = "None",
                    Contact = new Contact()
                    {
                        Name = "Poros",
                        Email = "development@poros.com",
                        Url = "Poros team"
                    }
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer",new string[]{}}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //app.UseExceptionHandler(builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            context.Response.AddApplicationError(error.Error.Message);
            //            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //        }
            //    });
            //});

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(@"Logs/api_log.txt");

            loggerFactory.WithFilter(new FilterLoggerSettings
                {
                    { "Microsoft", LogLevel.Warning },
                    { "System", LogLevel.Warning },
                }).AddSerilog(serilog.CreateLogger());

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poros APIs");
            });
        }
    }
}
