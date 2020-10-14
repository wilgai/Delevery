using Del.Common.Responses;
using Delevery.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Delevery.Prism.itemViewModel
{
  
    public class RestaurantItemViewModel : RestaurantResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectRestaurantCommand;

        public RestaurantItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectRestaurantCommand => _selectRestaurantCommand ?? (_selectRestaurantCommand = new DelegateCommand(SelectRestaurantAsync));

        private async void SelectRestaurantAsync()
        {
            NavigationParameters parameters = new NavigationParameters
    {
        { "restaurant", this }
    };

            await _navigationService.NavigateAsync(nameof(ProductsPage), parameters);

        }
    }

}
