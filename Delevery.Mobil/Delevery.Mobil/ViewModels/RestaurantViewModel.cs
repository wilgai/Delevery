using Del.Common.Responses;
using Del.Common.Services;
using Del.Common.Entities;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Delevery.Mobil.ViewModels
{
    
    public class RestaurantViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiservice _apiService;
        private ObservableCollection<Restaurant> _restaurants;

        public RestaurantViewModel(INavigationService navigationService, IApiservice apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Restaurants";
            LoadProductsAsync();
        }

        public ObservableCollection<Restaurant> Restaurants
        {
            get => _restaurants;
            set => SetProperty(ref _restaurants, value);
        }

        private async void LoadProductsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<Restaurant>(url,"/api","/Products");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            List<Restaurant> myRestaurnts = (List<Restaurant>)response.Result;
            Restaurants = new ObservableCollection<Restaurant>(myRestaurnts);
        }

    }
}
