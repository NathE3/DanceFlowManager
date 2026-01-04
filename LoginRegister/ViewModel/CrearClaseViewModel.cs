using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using System.Xml.Linq;



namespace InfoManager.ViewModel
{
    public partial class CrearClaseViewModel : ViewModelBase 
    {
        private ListadoAlumnosViewModel _listadoAlumnosViewModel;
        private readonly IHttpJsonProvider<ClaseDTO> _claseServiceToApi;

        [ObservableProperty]
        private string _nombre;

        [ObservableProperty]
        private string _descripcion;

        [ObservableProperty]
        private string _tipo;

        [ObservableProperty]
        private DateTime? _fechaClase;

        [ObservableProperty]
        private string _horaClase;

        public CrearClaseViewModel(IHttpJsonProvider<ClaseDTO> claseServiceToApi)
        {
            _claseServiceToApi = claseServiceToApi;
        }


        [RelayCommand]
        private async Task CrearClase() 
        {
            if (string.IsNullOrEmpty(Nombre) ||
               string.IsNullOrEmpty(Descripcion) ||
               string.IsNullOrEmpty(Tipo) || FechaClase == null)
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TimeSpan time = TimeSpan.Parse(HoraClase);
            DateTime fechaCompleta = FechaClase.Value.Date.Add(time);

            string token = App.Current.Services.GetService<LoginDTO>().Token;
            string idProfesorDesdeToken = ObtenerIdDesdeJwt(token);


            ClaseDTO clase = new()
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Tipo = Tipo,
                FechaClase = fechaCompleta,
                IdProfesor = idProfesorDesdeToken,
                Id_clase = "",
                AlumnosInscritos = []

            };
            try
            {
                var exito = await _claseServiceToApi.PostAsync(Constants.CLASE_URL, clase);

                if (exito)
                {
                    MessageBox.Show("Clase publicada correctamente.");
                }
                else
                {
                    MessageBox.Show("No se pudo publicar la clase. Revisa la consola.");
                }

                var mainViewModel = App.Current.Services.GetService<MainViewModel>();
                if (mainViewModel != null)
                {
                    mainViewModel.SelectedViewModel = mainViewModel.ClasesViewModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error durante el registro: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private static string ObtenerIdDesdeJwt(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return string.Empty;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == "sub");

                return claim?.Value ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        [RelayCommand]
        private async Task Cancelar(object? parameter)
        {
            if (_listadoAlumnosViewModel != null)
            {
                _listadoAlumnosViewModel.SelectedViewModel = null;
            }
        }

    }
}
