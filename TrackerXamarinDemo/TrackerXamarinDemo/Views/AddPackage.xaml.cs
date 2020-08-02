using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackerXamarinDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPackage : ContentPage
    {
        private ViewModels.AddPackageViewModel viewModel;
        IAnalyticsService service = DependencyService.Get<IAnalyticsService>();
        public AddPackage(Models.CompanyList companyList)
        {
            InitializeComponent();
            service?.LogEvent("AddPackagePage");
            BindingContext = viewModel = new ViewModels.AddPackageViewModel(Navigation, companyList);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Companies.Count == 0)
                viewModel.LoadCompanies.Execute(null);
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pickerValue = CompaniesPicker.SelectedItem;
            viewModel.Company = (Models.Company)pickerValue;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}