using Dollet.Core.Abstractions;
using Dollet.Core.Enums;

namespace Dollet.ViewModels.Categories
{
    public partial class IncomeCategoriesPageViewModel(IUnitOfWork unitOfWork) : CategoryBaseViewModel(unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        protected override async Task Appearing()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(CategoryType.Income);
            Categories.ReplaceRange(categories);
        }
    }
}