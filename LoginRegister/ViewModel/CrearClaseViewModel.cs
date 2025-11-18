using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace InfoManager.ViewModel
{
    public partial class CrearClaseViewModel : ViewModelBase 
    {

        private string _profesorId;
        private ListadoAlumnosViewModel _listadoAlumnosViewModel;
        private readonly IClaseServiceToApi _claseServiceToApi;

        [ObservableProperty]
        private string _nombre;

        [ObservableProperty]
        private string _descripcion;

        [ObservableProperty]
        private string _tipo;

        [ObservableProperty]
        private Date _fechaClase;

        [ObservableProperty]
        private ProfesorDTO _Profesor;

        public CrearClaseViewModel(IClaseServiceToApi claseServiceToApi)
        {
            _claseServiceToApi = claseServiceToApi;
        }

        public void SetIdProfesor(string id)
        {
            _profesorId = id;
        }

        [RelayCommand]
        private async Task CrearClase() 
        {
            if (string.IsNullOrEmpty(Nombre) ||
               string.IsNullOrEmpty(Descripcion) ||
               string.IsNullOrEmpty(Tipo))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            ClaseDTO clase = new()
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Tipo = Tipo,
                FechaClase = DateTime.Parse(FechaClase.ToString()),
                Id_Profesor = _profesorId,
                Id_clase = "",
                AlumnosInscritos = []

            };
            try
            {
                await _claseServiceToApi.PostClase(clase);

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
