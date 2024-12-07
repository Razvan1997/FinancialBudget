using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.ViewModels.Dtos
{
    public partial class CategoryDto : ObservableObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        [ObservableProperty]
        private bool _isSelected; 
        public decimal Budget { get; set; }
    }
}
