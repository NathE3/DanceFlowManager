using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace InfoManager.ViewModel
{
    public partial class RegistroViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _apellido;

        [ObservableProperty]
        private string _telefono;

        public LoginViewModel LoginViewModel { get; }

        private readonly IHttpJsonProvider<UserDTO> _httpJsonProvider;

        public RegistroViewModel(IHttpJsonProvider<UserDTO> httpJsonProvider, LoginViewModel loginViewModel)
        {
            _httpJsonProvider = httpJsonProvider;
            LoginViewModel = loginViewModel;
        }

        [RelayCommand]
        public async Task Registro()
        {
            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(UserName) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Apellido) || 
                string.IsNullOrEmpty(Telefono))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserRegistroDTO userRegistroDTO = new()
            {
                Name = Name,
                apellido = Apellido,
                UserName = UserName,
                Email = Email,
                Telefono = Telefono,
                Password = Password,
                IsProfesor = true

            };

            try
            {
                UserDTO user = await _httpJsonProvider.RegisterPostAsync(Constants.REGISTER_PATH, userRegistroDTO);
                if (user != null && user.Status == 0) {
                 App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().LoginViewModel;
                   

                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error durante el registro: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


