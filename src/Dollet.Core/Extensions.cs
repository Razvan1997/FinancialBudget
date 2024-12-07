using Dollet.Core.Abstractions.DomainServices;
using Dollet.Core.Helpers;
using Dollet.Core.Services;

namespace Dollet.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<PeriodsHelper>();

            services.AddTransient<IAccountDomainService, AccountDomainService>();

            return services;
        }
    }
}
