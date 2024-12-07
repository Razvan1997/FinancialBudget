using Dollet.Core.Abstractions;

namespace Dollet.ViewModels.Categories
{
    public partial class ExpenseCategoriesPageViewModel(IUnitOfWork unitOfWork) : CategoryBaseViewModel(unitOfWork) { }
}