using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.ViewModels.Dtos;
using Plugin.Maui.OCR;
using System.Text.RegularExpressions;


namespace Dollet.ViewModels.Transactions.Expenses
{
    public partial class AddExpensePageViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        private Account _selectedAccount;
        public decimal Amount { get; set; }
        public CategoryDto SelectedCategory { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime MaximumDate { get; } = DateTime.Now;

        public ObservableRangeCollection<Account> Accounts { get; } = [];
        public ObservableRangeCollection<CategoryDto> Categories { get; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = false;
            var context = _unitOfWork.GetApplicationContext();

            var accounts = await _unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password) ;

            var categories = await _unitOfWork.AccountCategoryRepository.GetCategoriesByAccountIdAsync(accounts.FirstOrDefault().Id);

            Accounts.ReplaceRange(accounts);
            foreach (var category in categories)
            {
                var item = _unitOfWork.CategoryRepository.GetCategoryByIdAsync(category.CategoryId).Result;
                var categoryDto = new CategoryDto()
                {
                    Id = item.Id,
                    Color = item.Color,
                    Icon = item.Icon,
                    Name = item.Name,
                    IsSelected = false,
                    Budget = category.Budget
                };
                Categories.Add(categoryDto);
            }

            SelectedAccount = accounts.FirstOrDefault(x => x.IsDefault);

            await OcrPlugin.Default.InitAsync();
        }

        [RelayCommand]
        async Task AddExpense()
        {
            var selectedAccount = Accounts.FirstOrDefault(x => x.Id == SelectedAccount.Id);
            var selectedCategory = Categories.FirstOrDefault( x=> x.Id == SelectedCategory.Id);

            if (SelectedAccount == null)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Trebuie sa selectezi contul.", "OK");
                return;
            }

            if (Amount > selectedCategory.Budget)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Suma introdusa depaseste bugetul pe categoria selectata.", "OK");
                return;
            }
            else
            {
                selectedAccount.Amount -= Amount;
                _unitOfWork.AccountRepository.Update(selectedAccount);
                _unitOfWork.ExpensesRepository.Add(new Expense
                {
                    Amount = Amount,
                    AccountId = SelectedAccount.Id,
                    CategoryId = SelectedCategory.Id,
                    Date = Date,
                    Comment = Comment
                });

                try
                {
                    await _unitOfWork.CommitAsync();
                    await Toast
                   .Make("Added", ToastDuration.Long)
                   .Show();

                    await Shell.Current.GoToAsync("..");
                }
                catch(Exception ex)
                {

                }
            }
        }

        [RelayCommand]
        async Task<string> MakeFoto()
        {
            var photoPath = await TakePhotoAsync();
            if (photoPath != null)
            {
                var result = await ExtractTotalPriceFromImagePath(photoPath);
            }
            return "Nu s-a putut face poza.";
        }

        public async Task<string> ExtractTotalPriceFromImagePath(string imagePath)
        {
            try
            {
                var imageBytes = await File.ReadAllBytesAsync(imagePath);

                var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageBytes);

                if (ocrResult.Success)
                {
                    await Application.Current.MainPage.DisplayAlert("",ocrResult.AllText,"OK");
                    var totalPrice = ExtractTotalPrice(ocrResult.AllText);

                    if (!string.IsNullOrEmpty(totalPrice))
                    {
                        await Application.Current.MainPage.DisplayAlert("Preț Total Detectat", totalPrice, "OK");
                        return totalPrice;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Preț Neidentificat", "Nu s-a găsit prețul total.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("OCR Eșuat", "Nu s-a putut extrage textul.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", ex.Message, "OK");
            }

            return "Prețul nu a fost găsit.";
        }

        public async Task<string> TakePhotoAsync()
        {
            try
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using var stream = await photo.OpenReadAsync();
                    using var newStream = File.OpenWrite(localFilePath);
                    await stream.CopyToAsync(newStream);

                    return localFilePath;  // Returnează calea imaginii
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la accesarea camerei: {ex.Message}");
            }
            return null;
        }

        private string ExtractTotalPrice(string ocrText)
        {
            // Modificare Regex pentru a permite "TOTAL" sau "TOTAL LEI"
            var match = Regex.Match(ocrText, @"\bTOTAL(?:\s*LEI)?\s*([\d.,]+)");

            if (match.Success)
            {
                return match.Groups[1].Value;  // Returnează doar valoarea numerica
            }
            return null;
        }

    }
}
