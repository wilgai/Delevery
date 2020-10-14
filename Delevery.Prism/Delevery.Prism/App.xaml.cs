using Prism;
using Prism.Ioc;
using Delevery.Prism.ViewModels;
using Delevery.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Del.Common.Services;
using Syncfusion.Licensing;


namespace Delevery.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzMwNDE5QDMxMzgyZTMzMmUzME9QSzgzenNkUitZSTN1N2RSemM2bk1MaFJUcThpMFpnci9QU3JkV2R1ZVE9");
            
            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(RestaurantsPage)}");
            //await NavigationService.NavigateAsync($"NavigationPage/{nameof(OnSaleMasterDetailPage)}");

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiservice, ApiService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSaleMasterDetailPage, OnSaleMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowCarPage, ShowCarPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowHistoryPage, ShowHistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<RestaurantsPage, RestaurantsPageViewModel>();
        }
    }
}
