using Isatol.Tracker;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Services;
using Xamarin.Forms;

namespace TrackerXamarinDemo.ViewModels
{
    public class TrackerViewModel : BaseViewModel
    {
        private TrackService TrackService;
        private readonly INavigation _navigation;
        public ObservableCollection<Models.Package> Packages { get; set; }
        public ObservableCollection<Isatol.Tracker.Models.TrackingModel> TrackingModel { get; set; }    
        private Models.CompanyList CompanyList { get; set; }
        public TrackerViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Title = "Package Tracker";            
            Packages = new ObservableCollection<Models.Package>();
            TrackingModel = new ObservableCollection<Isatol.Tracker.Models.TrackingModel>();
            TrackService = new TrackService();
            CompanyList = new Models.CompanyList();
            //LoadUserPackages = new Command(async () => await ExecuteLoadUserPackages(User.UserID));
        }

        #region Properties
        
        #endregion

        #region Commands
        public Command LoadUserPackages
        {
            get
            {
                return new Command(async () =>
                {
                    await ExecuteLoadUserPackages();                    
                });
            }
        }

        public Command NavigateToAddPackage
        {
            get
            {
                return new Command(async () =>
                {
                    await ExecuteNavigateToAddPackage();
                });
            }
        }

        //public Command TapPackage
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            await TapPackageAction();
        //        });
        //    }
        //}
        #endregion

        #region Actions Commnads
        async Task ExecuteLoadUserPackages() 
        {
            IsBusy = true;
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Cargando...");
                try
                {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://isatolpackagetrackerapi.azurewebsites.net/api/Companies/GetCompanies")
                };
                var response = await clientTrackerAPI.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                Models.CompanyList companyResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.CompanyList>(responseContent);
                CompanyList = companyResponse;
                Packages.Clear();
                TrackingModel.Clear();
                var packages = await App.Database.GetPackages();                
                if(packages.Count > 0)
                {

                    packages.ForEach(async package => 
                    {
                        Isatol.Tracker.Models.TrackingModel trackingModel = await TrackService.GetTracking(package.CompanyID, package.TrackingNumber);
                        Packages.Add(new Models.Package 
                                {
                                    CompanyID = package.CompanyID,
                                    Event = trackingModel.Status,
                                    PackageName = package.Name,
                                    TrackingNumber = package.TrackingNumber,
                                    Name = companyResponse.Companies.Where(c => c.CompanyID == package.CompanyID).Select(c => c.Name).FirstOrDefault(),
                                    TrackingModel = trackingModel,
                                    ImageSource = companyResponse.Companies.Where(c => c.CompanyID == package.CompanyID).Select(c => c.LogoImageSource).FirstOrDefault()
                                }); 
                    });
                }                
                    //    Packages.Add(new Database.Database.Package 
                    //    {

                    //    });
                    //    //Packages.Add(new Models.Package 
                    //    //{
                    //    //    CompanyID = package.CompanyID,
                    //    //    Date = package.Date,
                    //    //    Event = package.Event,
                    //    //    ImageSource = imgSource,
                    //    //    Name = package.Name,
                    //    //    PackageID = package.PackageID,
                    //    //    PackageName = package.PackageName,
                    //    //    TrackingNumber = package.TrackingNumber
                    //    //});
                    //});                                    
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    IsBusy = false;
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
        }

        async Task ExecuteNavigateToAddPackage() 
        {
            await _navigation.PushModalAsync(new Views.AddPackage(CompanyList));
        }

        //public async Task TapPackageAction() 
        //{

        //}
        #endregion
    }
}
