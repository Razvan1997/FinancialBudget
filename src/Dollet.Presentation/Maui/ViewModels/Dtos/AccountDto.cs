using CommunityToolkit.Mvvm.ComponentModel;

namespace Dollet.ViewModels.Dtos
{
    public partial class AccountDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        decimal amount;

        [ObservableProperty]
        string name, description, icon, color, currency, username, password;

        [ObservableProperty]
        bool isHidden, isDefault;

        public override string ToString()
        {
            return Name;
        }
    }
}