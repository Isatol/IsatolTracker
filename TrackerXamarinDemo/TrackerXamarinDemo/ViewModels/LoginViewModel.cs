using SQLite;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackerXamarinDemo.Database;
using TrackerXamarinDemo.Models;
using TrackerXamarinDemo.Views;
using Xamarin.Forms;

namespace TrackerXamarinDemo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigation Navigation;
        private SQLiteAsyncConnection _conn;
        //public Command LoginCommand { get; set; }
        public LoginViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
#if DEBUG
            Email = "iot_93@hotmail.com";
            Password = "isa";
#endif
            _conn = DependencyService.Get<ISQLiteDB>().GetConnection();
        }

        #region Properties

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (SetProperty(ref _email, value))
                {
                    ((Command)LoginCommand).ChangeCanExecute();
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(ref _password, value))
                {
                    ((Command)LoginCommand).ChangeCanExecute();
                }
            }
        }

        private bool _isShowCancel;
        public bool IsShowCancel
        {
            get { return _isShowCancel; }
            set { SetProperty(ref _isShowCancel, value); }
        }

        #endregion


        #region Commands

        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get { return _loginCommand = _loginCommand ?? new Command(async () => await LoginAction(), CanLoginAction); }
        }

        private ICommand _cancelLoginCommand;
        public ICommand CancelLoginCommand
        {
            get { return _cancelLoginCommand = _cancelLoginCommand ?? new Command(async () => await CancelLoginAction()); }
        }

        //private ICommand _forgotPasswordCommand;
        //public ICommand ForgotPasswordCommand
        //{
        //    get { return _forgotPasswordCommand = _forgotPasswordCommand ?? new Command(ForgotPasswordAction); }
        //}

        private ICommand _newAccountCommand;
        public ICommand NewAccountCommand
        {
            get { return _newAccountCommand = _newAccountCommand ?? new Command(NewAccountAction); }
        }

        #endregion


        #region Methods

        bool CanLoginAction()
        {
            //Perform your "Can Login?" logic here...

            if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.Password))
                return false;

            return true;
        }

        async Task LoginAction()
        {
            IsBusy = true;
            Dictionary<string, object> loginModel = new Dictionary<string, object> 
            {
                {"Email", Email },
                {"Password", Password }
            };
            string prepareJsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
            StringContent stringContent = new StringContent(prepareJsonContent, Encoding.UTF8, "application/json");     
            IsBusy = false;
            using (clientTrackerAPI)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    Content = stringContent,
                    RequestUri = new Uri(clientTrackerAPI.BaseAddress, "Auth/Login")
                };
                IsShowCancel = true;
                HttpResponseMessage responseLogin = await clientTrackerAPI.SendAsync(requestMessage);
                string jsonResponse = await responseLogin.Content.ReadAsStringAsync();
                LoginResponse loginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);
                if(loginResponse != null)
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(loginResponse.Token);
                    Models.User user = new User();
                    var claimsList = token.Claims.ToList();
                    user.UserID = claimsList.Where(c => c.Type == "userID").Select(c => Convert.ToInt32(c.Value)).First();
                    user.Name = claimsList.Where(c => c.Type == "name").Select(c => c.Value).First();
                    user.Email = claimsList.Where(c => c.Type == "email").Select(c => c.Value).First();
                    user.ExpToken = claimsList.Where(c => c.Type == "exp").Select(c => ConvertFromUnixTimestamp(Convert.ToDouble(c.Value))).First();
                    App.Current.Properties.Add("User", user);
                    //await App.Database.Insert(user);
                    await _conn.InsertAsync(user);
                    IsShowCancel = false;
                    Application.Current.MainPage = new NavigationPage(new Views.Tracker());
                    //await Navigation.InsertPageBefore(new Tracker());                    
                }
                IsShowCancel = false;
            }            
            //TODO - perform your login action + navigate to the next page

            //Simulate an API call to show busy/progress indicator            
            //await Task.Delay(1000).ContinueWith((t) => IsBusy = false);

            ////Show the Cancel button after X seconds
            //await Task.Delay(5000).ContinueWith((t) => IsShowCancel = true);
        }

        async Task CancelLoginAction()
        {
            //TODO - perform cancellation logic

            IsBusy = false;
            IsShowCancel = false;
        }

        //void ForgotPasswordAction()
        //{
        //    //TODO - navigate to your forgotten password page
        //    //Navigation.PushAsync(XXX);
        //}

        void NewAccountAction()
        {
            //TODO - navigate to your registration page
            //Navigation.PushAsync(XXX);
        }

        #endregion

    }
}
