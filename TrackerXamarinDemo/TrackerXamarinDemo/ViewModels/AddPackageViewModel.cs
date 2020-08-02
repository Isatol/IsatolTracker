using Acr.UserDialogs;
using Isatol.Tracker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Models;
using TrackerXamarinDemo.Services;
using Xamarin.Forms;

namespace TrackerXamarinDemo.ViewModels
{
    public class AddPackageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private TrackService TrackService;
        public ObservableCollection<Models.Company> Companies { get; set; }
        private Models.CompanyList _companyList;
        public AddPackageViewModel(INavigation navigation, Models.CompanyList companyList)
        {
            _navigation = navigation;
            Companies = new ObservableCollection<Models.Company>();
            _companyList = companyList;
            TrackService = new TrackService();
#if DEBUG
            TrackingNumber = "005588955041C705695585";
            PackageName = "Disco SSD";
#endif
        }

        #region Properties
        private string _trackingNumber;
        public string TrackingNumber
        {
            get => _trackingNumber;
            set
            {
                if(SetProperty(ref _trackingNumber, value))
                {
                    ((Command)AddPackageCommand).ChangeCanExecute();
                }
            }
        }

        private string _packageName;
        public string PackageName
        {
            get => _packageName;
            set
            {
                SetProperty(ref _packageName, value);                             
            }
        }

        private Models.Company _company;
        public Models.Company Company
        {
            get => _company;
            set
            {
                SetProperty(ref _company, value);
            }
        }
        #endregion

        #region Commands        
        public Command AddPackageCommand
        {
            get
            {
                return new Command(async () => await AddPackage());
            }
        }        

        public Command LoadCompanies
        {
            get
            {
                return new Command(async () =>
                {
                    await LoadCompaniesAction();
                });
            }
        }
        #endregion

        #region Actions
        async Task AddPackage()
        {
            if (string.IsNullOrEmpty(PackageName) || Company == null)
                //UserDialogs.Instance.Alert("El nombre del paquete y la comañía no pueden estar vacíos", "Completar campos faltantes", "Aceptar");                
                UserDialogs.Instance.Toast("Los campos no pueden estar vacíos");
            else
            {
                UserDialogs.Instance.ShowLoading("Agregando...");
                var package = await App.Database.GetPackage(TrackingNumber);
                if (package == null)
                {
                    var trackingModel = await TrackService.GetTracking(Company.CompanyID, TrackingNumber);
                    int identityPackage = await App.Database.InsertPackage(new Database.Database.Package
                    {
                        CompanyID = Company.CompanyID,
                        Name = PackageName,
                        TrackingNumber = TrackingNumber
                    });
                    var lastPackageAdded = await App.Database.GetPackage(TrackingNumber);
                    if (trackingModel.TrackingDetails.Count > 0)
                    {
                        await App.Database.InsertLastPackageUpdate(new Database.Database.LastPackageUpdate
                        {
                            PackageID = lastPackageAdded.PackageID,
                            Date = trackingModel.TrackingDetails[0].Date,
                            Event = trackingModel.TrackingDetails[0].Event,
                            Messages = trackingModel.TrackingDetails[0].Messages
                        });
                    }
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast($"Paquete {PackageName} agregado correctamente");
                    await _navigation.PopModalAsync();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync($"{PackageName} ya lo tienes agregado mediante el mismo número de rastreo", "Ya tienes agregado este paquete", "Aceptar");
                }                
            }
        }                     
        async Task LoadCompaniesAction()
        {
            IsBusy = true;
            Companies.Clear();
            _companyList.Companies.ForEach(c => 
            {
                Companies.Add(c);
            });
            IsBusy = false;
        }
        #endregion
    }
}
