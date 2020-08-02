using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackerXamarinDemo.Models;
using Xamarin.Forms;

namespace TrackerXamarinDemo.ViewModels
{    
    public class TrackerDetailViewModel : BaseViewModel
    {
        private readonly INavigation Navigation;
        // public ObservableCollection<Models.Package> Packages { get; set; }        
        public ObservableCollection<Isatol.Tracker.Models.TrackingDetails> TrackingDetails { get; set; }                
        private Isatol.Tracker.Models.TrackingModel _trackingModel;
        private Models.Package _package;
        public TrackerDetailViewModel(INavigation navigation, string packageName, Models.Package package, Isatol.Tracker.Models.TrackingModel trackingModel)
        {
            Navigation = navigation;            
            TrackingDetails = new ObservableCollection<Isatol.Tracker.Models.TrackingDetails>();
            _trackingModel = trackingModel;
            Title = packageName;
            _package = package;
        }

        #region Commands
        public Command TrackPackageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await TrackPackageAction();
                });
            }
        }

        public Command DeletePackage
        {
            get
            {
                return new Command(async () => 
                {
                    await DeletePackageAction();
                });
            }
        }
        #endregion

        #region Properties
        private bool _isVisibleDelivered;
        public bool IsVisibleDelivered 
        {
            get => _isVisibleDelivered;
            set 
            {
                SetProperty(ref _isVisibleDelivered, value);
            }
        }

        private bool _isVisibleInTransit;
        public bool IsVisibleInTransit
        {
            get => _isVisibleInTransit;
            set
            {
                SetProperty(ref _isVisibleInTransit, value);
            }
        }

        private string _currentStatus;
        public string CurrentStatus
        {
            get => _currentStatus;           
            set
            {
                SetProperty(ref _currentStatus, value);
            }
        }

        private string _estimateDeliveryDate;
        public string EstimateDeliveryDate
        {
            get => _estimateDeliveryDate;
            set
            {
                SetProperty(ref _estimateDeliveryDate, value);
            }
        }

        private bool _isVisiblePage;
        public bool IsVisiblePage
        {
            get => _isVisiblePage;
            set
            {
                SetProperty(ref _isVisiblePage, value);
            }
        }

        #endregion

        #region Actions

        async Task DeletePackageAction() 
        {
            UserDialogs.Instance.ShowLoading("Eliminando...", MaskType.Gradient);
            try
            {
                var package = await App.Database.GetPackage(_package.TrackingNumber);
                await App.Database.DeletePackage(package.PackageID);
                UserDialogs.Instance.Toast($"{_package.Name} se ha eliminado correctamente");
                UserDialogs.Instance.HideLoading();
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast("Ha ocurrido un error al eliminar el paquete, intente más tarde");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();                
            }
        }
        async Task TrackPackageAction()
        {            
            IsBusy = true;
            IsVisiblePage = false;
            try
            {
                TrackingDetails.Clear();
                    //TrackingModel.Clear();
                //HttpRequestMessage requestMessage = new HttpRequestMessage
                //{
                //  Method = HttpMethod.Get,
                //  RequestUri = new Uri(clientTrackerAPI.BaseAddress, $"Track/GetTrackingModel?companyID={Package.CompanyID}&trackingNumber={Package.TrackingNumber}")
                //};
                //var response = await clientTrackerAPI.SendAsync(requestMessage);
                //var responseContent = await response.Content.ReadAsStringAsync();
                //Models.Tracking trackingModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Tracking>(responseContent);
                IsVisibleDelivered = _trackingModel.Delivered;
                IsVisibleInTransit = !_trackingModel.Delivered;
                CurrentStatus = _trackingModel.Status;
                if (_trackingModel.Delivered || _trackingModel == null)
                {
                  EstimateDeliveryDate = "Sin fecha estimada de llegada";
                }
                else
                {
                    EstimateDeliveryDate = _trackingModel.EstimateDelivery == null ? "Sin fecha estimada de llegada" : _trackingModel.EstimateDelivery.Value.ToString("dd-MM-yyyy HH:mm");
                }
                _trackingModel.TrackingDetails.ForEach(detail => 
                {
                    TrackingDetails.Add(new Isatol.Tracker.Models.TrackingDetails
                    {
                        Date = detail.Date,
                        Event = detail.Event,
                        Messages = detail.Messages,
                        //StringDate = detail.Date.Value.ToString("dd-MM-yyyy HH:mm")
                    });
                });                                                                   
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                IsBusy = false;
                IsVisiblePage = true;
            }
        }
        #endregion
    }
}
