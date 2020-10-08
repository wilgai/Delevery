using Delevery.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delevery.Prism.ViewModels
{
    public class ShowCarPageViewModel : ViewModelBase
    {
        public ShowCarPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            Title = Languages.ShowShoppingCar;
        }
    }
}
