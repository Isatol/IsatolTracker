using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackerXamarinDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerDetail : ContentPage
    {
        private ViewModels.TrackerDetailViewModel viewModel;
        IAnalyticsService service = DependencyService.Get<IAnalyticsService>();
        public TrackerDetail(string packageName, Models.Package package,  Isatol.Tracker.Models.TrackingModel trackingModel)
        {
            InitializeComponent();
            service?.LogEvent("TrackerDetailPage");
            BindingContext = viewModel = new ViewModels.TrackerDetailViewModel(Navigation, packageName, package, trackingModel);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.TrackPackageCommand.Execute(null);
            //CreateGrid();
            //if (viewModel.Package == null)
            //    viewModel.IsBusy = true;
        }        
    }
}