using CommunityToolkit.Mvvm.Input;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.Helpers;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

namespace InfoManager.ViewModel
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IHttpJsonProvider<UserDTO> _httpJsonProvider;

       

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _password;

        public LoginViewModel(IHttpJsonProvider<UserDTO> httpJsonProvider)
        {
            _httpJsonProvider = httpJsonProvider;       
        }

        [RelayCommand]
        public async Task Login()
        {
            App.Current.Services.GetService<LoginDTO>().Email = Name;
            App.Current.Services.GetService<LoginDTO>().Password = Password;
            App.Current.Services.GetService<LoginDTO>().IsProfesor = true;

            try
            {
                UserDTO response = await _httpJsonProvider.LoginPostAsync(Constants.LOGIN_PATH, App.Current.Services.GetService<LoginDTO>());

                if (response != null && response.Status == 0)
                {
                    App.Current.Services.GetService<LoginDTO>().Token = response.Token;

                    App.Current.Services.GetService<MainViewModel>();
                    App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().ClasesViewModel;                                
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void Register()
        {

            var mainWindow = App.Current.Services.GetService<MainViewModel>();
            mainWindow.SelectedViewModel = App.Current.Services.GetService<MainViewModel>().RegistroViewModel;
        }
    }
}

