﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;

namespace Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(IAssemblyMarker).Assembly;
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddValidatorsFromAssemblies(new[] { applicationAssembly });


            services.AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(applicationAssembly);
            });


            return services;
        }
    }
}
