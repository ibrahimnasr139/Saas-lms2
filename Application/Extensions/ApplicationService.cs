using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class ApplicationService
    {
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(cfg => { }, typeof(ApplicationService).Assembly);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationService).Assembly));
            builder.Services.AddValidatorsFromAssembly(typeof(ApplicationService).Assembly)
                .AddFluentValidationAutoValidation();
            builder.Services.AddHttpContextAccessor();
        }
    }
}
