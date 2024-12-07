using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DAL.Helpers;
using Dollet.Infrastructure.DAL;
using Dollet.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Core.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddDal(this IServiceCollection services)
        {
            services.AddDbContext<DolletDbContext>(options =>
            {
                var name = DatabasePath.GetPath("dollet.db");
                //DatabasePath.ExportDatabase(name);
                options.UseSqlite($"Filename={name}");
                //options.UseSqlite(@"Data Source=D:/dollet.db");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAppDataRepository, AppDataRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IExpensesRepository, ExpensesRepository>();
            services.AddTransient<IIncomesRepository, IncomesRepository>();
            services.AddTransient<IUserRepository, UsersRepository>();
            services.AddTransient<IAccountCategoryRepository, AccountCategoryRepository>();

            services.AddTransient(provider =>
                new Lazy<IAccountRepository>(() => provider.GetRequiredService<IAccountRepository>()));
            services.AddTransient(provider =>
                new Lazy<IAppDataRepository>(() => provider.GetRequiredService<IAppDataRepository>()));
            services.AddTransient(provider =>
                new Lazy<ICategoryRepository>(() => provider.GetRequiredService<ICategoryRepository>()));
            services.AddTransient(provider =>
                new Lazy<ICurrencyRepository>(() => provider.GetRequiredService<ICurrencyRepository>()));
            services.AddTransient(provider =>
                new Lazy<IExpensesRepository>(() => provider.GetRequiredService<IExpensesRepository>()));
            services.AddTransient(provider =>
                new Lazy<IIncomesRepository>(() => provider.GetRequiredService<IIncomesRepository>()));
            services.AddTransient(provider =>
               new Lazy<IUserRepository>(() => provider.GetRequiredService<IUserRepository>()));
            services.AddTransient(provider =>
               new Lazy<IAccountCategoryRepository>(() => provider.GetRequiredService<IAccountCategoryRepository>()));

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }        
    }
}
