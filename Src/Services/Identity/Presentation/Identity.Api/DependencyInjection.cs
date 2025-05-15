using System.Reflection;

namespace Identity.Api
{
    public static class DependencyInjection
    {
        private readonly static Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

  

        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            return services;
        }

     
    }
}
