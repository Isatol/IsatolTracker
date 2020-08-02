using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Services;
using TrackerXamarinDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackerXamarinDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tracker : ContentPage
    {
        private TrackerViewModel viewModel;
        IAnalyticsService service = DependencyService.Get<IAnalyticsService>();
        public Tracker()
        {
            InitializeComponent();
            service?.LogEvent("TrackerPage");
            BindingContext = viewModel = new ViewModels.TrackerViewModel(Navigation);            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();            
            Task.Run(async () => 
            {
                var packages = await App.Database.GetPackages();
                if(packages.Count == 0)
                {
                    viewModel.Packages.Clear();
                }
                if (viewModel.Packages.Count == 0)
                    viewModel.IsBusy = true;
            });            
        }
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (Models.Package)layout.BindingContext;
            Navigation.PushAsync(new Views.TrackerDetail(item.PackageName, item, item.TrackingModel));
            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }
    }
}