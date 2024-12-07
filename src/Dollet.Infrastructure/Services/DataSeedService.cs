using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Services;
using Dollet.Core.Constants;
using Dollet.Core.Entities;
using Dollet.Core.Enums;
using System.Xml.Linq;

namespace Dollet.Infrastructure.Services
{
    internal class DataSeedService(IUnitOfWork unitOfWork, IFreeCurrencyService freeCurrencyService) : IDataSeedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFreeCurrencyService _freeCurrencyService = freeCurrencyService;

        public async Task SeedCategoriesAsync()
        {
            if (!await _unitOfWork.CategoryRepository.AnyAsync())
            {
                var expenseCategories = new List<Category>
                {
                    new() { IndexOrder = 0, Name = "Băcănii", Icon = MaterialDesignIcons.Local_grocery_store, Color = "#d2b7b7" },
                    new() { IndexOrder = 1, Name = "Activitate", Icon = MaterialDesignIcons.Sports_baseball, Color = "#d08c60" },
                    new() { IndexOrder = 2, Name = "Divertisment", Icon = MaterialDesignIcons.Sports_bar, Color = "#e76f51" },
                    new() { IndexOrder = 3, Name = "Restaurante", Icon = MaterialDesignIcons.Restaurant, Color = "#819a78" },
                    new() { IndexOrder = 4, Name = "Casa", Icon = MaterialDesignIcons.House, Color = "#7782b0" },
                    new() { IndexOrder = 5, Name = "Educatie", Icon = MaterialDesignIcons.Science, Color = "#606a9f" },
                    new() { IndexOrder = 6, Name = "Abonari", Icon = MaterialDesignIcons.Subscriptions, Color = "#e39e83" },
                    new() { IndexOrder = 7, Name = "Cafele", Icon = MaterialDesignIcons.Local_cafe, Color = "#997b66" },
                    new() { IndexOrder = 8, Name = "Altele", Icon = MaterialDesignIcons.Done_all, Color = "#a76e6e" }
                };

                var incomeCategories = new List<Category>
                {
                    new() { IndexOrder = 0, Name = "Salariu", Icon = MaterialDesignIcons.Paid, Color = "#d2b7b7", Type = CategoryType.Income },
                    new() { IndexOrder = 1, Name = "Cadouri", Icon = MaterialDesignIcons.Redeem, Color = "#d08c60", Type = CategoryType.Income },
                    new() { IndexOrder = 2, Name = "Interese", Icon = MaterialDesignIcons.Percent, Color = "#e76f51", Type = CategoryType.Income },
                    new() { IndexOrder = 3, Name = "Altele", Icon = MaterialDesignIcons.Done_all, Color = "#a76e6e", Type = CategoryType.Income }
                };

                _unitOfWork.CategoryRepository.AddMany(expenseCategories);
                _unitOfWork.CategoryRepository.AddMany(incomeCategories);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SeedCurrenciesAsync()
        {
            if (!await _unitOfWork.CurrencyRepository.AnyAsync())
            {
                var currencies = await _freeCurrencyService.GetCurrenciesAsync();

                var currenciesToAdd = currencies
                    .Select(x => new Currency
                    {
                        Code = x.Key,
                        Name = x.Value.Name,
                        IsDefault = x.Key == "RON"
                    })
                    .ToList();

                _unitOfWork.CurrencyRepository.AddMany(currenciesToAdd);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SeedCurrencyValuesAsync(string currency = "RON")
        {
            var currencyValues = await _freeCurrencyService.GetCurrencyValuesAsync(currency);

            var valuesToAdd = currencyValues.Select(x => new CurrencyValue
            {
                CodeFrom = currency,
                CodeTo = x.Key,
                Value = x.Value
            });

            await _unitOfWork.CurrencyRepository.DeleteValuesAsync();
            _unitOfWork.CurrencyRepository.AddManyValues(valuesToAdd);
            await _unitOfWork.CommitAsync();
        }

        public async Task SeedUsersAsync()
        {
            if (!await _unitOfWork.UserRepository.AnyAsync())
            {
                var users = new List<Users> 
                { 
                    new Users() 
                    { 
                        Id = 1,
                        Name = "Admin", 
                        Password = "Admin", 
                        Role = UserType.Admin 
                    },
                    new Users()
                    {
                        Id = 2,
                        Name = "normal1",
                        Password = "normal1",
                        Role = UserType.Normal
                    },
                    new Users()
                    {
                        Id = 3,
                        Name = "normal2",
                        Password = "normal2",
                        Role = UserType.Normal
                    },
                };
                _unitOfWork.UserRepository.AddMany(users);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
