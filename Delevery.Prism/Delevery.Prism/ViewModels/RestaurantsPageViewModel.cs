using Del.Common.Responses;
using Del.Common.Services;
using Delevery.Prism.Helpers;
using Delevery.Prism.itemViewModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Delevery.Prism.ViewModels
{
    public class RestaurantsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiservice _apiService;
        private ObservableCollection<RestaurantItemViewModel> _restaurants;
        private bool _isRunning;
        private string _search;
        private List<RestaurantResponse> _myRestaurants;
        private DelegateCommand _searchCommand;


        public RestaurantsPageViewModel(INavigationService navigationService, IApiservice apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Places;
            LoadRestaurantsAsync();
        }
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowRestaurants));
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowRestaurants();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<RestaurantItemViewModel> Restaurants
        {
            get => _restaurants;
            set => SetProperty(ref _restaurants, value);
        }


        private async void LoadRestaurantsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetListAsync<RestaurantResponse>(
                url,
                "/api",
                "/Restaurants");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myRestaurants = (List<RestaurantResponse>)response.Result;
            ShowRestaurants();

        }
        private void ShowRestaurants()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Restaurants = new ObservableCollection<RestaurantItemViewModel>(_myRestaurants.Select(r => new RestaurantItemViewModel(_navigationService)
                {
                    
                    Name = r.Name,
                    Address = r.Address

                })
                    .ToList());
            }
            else
            {
                Restaurants = new ObservableCollection<RestaurantItemViewModel>(_myRestaurants.Select(r => new RestaurantItemViewModel(_navigationService)
                {
                   
                    Name = r.Name,
                    Address=r.Address
                    
                   
                })
                    .Where(p => p.Name.ToLower().Contains(Search.ToLower()))
                    .ToList());
            }
        }


    }
}
