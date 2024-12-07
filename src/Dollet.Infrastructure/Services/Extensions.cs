using Dollet.Core.Abstractions.Services;
using freecurrencyapi;
using Microsoft.Extensions.Configuration;

namespace Dollet.Infrastructure.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(sp => new Freecurrencyapi("fca_live_g1Fta4EqDbFeAPwR3yETQE7SacgvlyfM9qZFGD1p"));

            services.AddSingleton<IDateTimeRangeService, DateTimeRangeService>();
            services.AddTransient<IDataSeedService, DataSeedService>();
            services.AddTransient<IFreeCurrencyService, FreeCurrencyService>();

            return services;
        }
    }
}