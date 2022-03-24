using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Crowe.Template.Command.Services.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;

        public AuthorizeCheckOperationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuthorize =
                context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });


                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                         {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Swagger.OAuth2"
                                }
                            },
                            new [] { _configuration["Idp:Scope"] }
                        }
                    }
                };
            }
        }
    }
}
