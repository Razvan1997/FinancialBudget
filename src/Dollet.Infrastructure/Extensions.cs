using Dollet.Core.DAL;
using Dollet.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Dollet.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDal();
            services.AddServices(configuration);

            return services;
        }   
    }
}