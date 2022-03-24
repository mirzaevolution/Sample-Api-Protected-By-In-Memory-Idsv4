using Crowe.Template.Command.Services.Filters;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Sample.Api
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = Configuration["Idp:BaseAddress"];
                    options.Audience = Configuration["Idp:Audience"];
                    options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
                    options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
                });
            var authorizationCodeFlow = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{Configuration["Idp:BaseAddress"]}/connect/authorize"),
                TokenUrl = new Uri($"{Configuration["Idp:BaseAddress"]}/connect/token")
            };
            authorizationCodeFlow.Scopes = new Dictionary<string, string>
            {
                { Configuration["Idp:Scope"], Configuration["Idp:Scope"] }
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample.Api", Version = "v1" });
                c.AddSecurityDefinition("Swagger.OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = authorizationCodeFlow
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Cors", config =>
                {
                    config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Cors");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample.Api v1");
                c.OAuthClientId(Configuration["Idp:ClientId"]);
                c.OAuthClientSecret(Configuration["Idp:ClientSecret"]);
                c.OAuthAppName("Sample.Api");
                c.OAuthUsePkce();
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
