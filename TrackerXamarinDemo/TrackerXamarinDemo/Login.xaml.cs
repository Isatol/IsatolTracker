using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackerXamarinDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();            
            BindingContext = new LoginViewModel(Navigation);
        }

        protected override void OnDisappearing() 
        {
            Navigation.PopAsync();
        }
    }
}