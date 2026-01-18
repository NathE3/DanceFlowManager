using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;


namespace InfoManager.ViewModel;

public partial class ClasesViewModel : ViewModelBase
{
    private readonly IHttpJsonProvider<ClaseDTO> _httpJsonProvider;
    private readonly IHttpJsonProvider<AlumnoDTO> _httpJsonProviderAlumnos;

    private ClaseDTO _claseSeleccionada;

    [ObservableProperty]
    private ObservableCollection<ClaseDTO> _clasesMostradas;

    private int _alumnosContados;

    public ClasesViewModel(IHttpJsonProvider<ClaseDTO> httpJsonProvider, IHttpJsonProvider<AlumnoDTO> httpJsonProviderAlumnos)
    {
        _httpJsonProvider = httpJsonProvider;
        _clasesMostradas = [];
        _httpJsonProviderAlumnos = httpJsonProviderAlumnos;
    }


    public ClaseDTO ClaseSeleccionada
    {
        get => _claseSeleccionada;
        set
        {
            _claseSeleccionada = value;
            OnPropertyChanged();
        }
    }

    public override async Task LoadAsync()
    {
        try
        {
         
            ClasesMostradas.Clear();

            var listaClases = await _httpJsonProvider.GetAsync(Constants.CLASE_URL);

            if (listaClases != null)
            {
                var ordenadas = listaClases.OrderBy(d => d.IdProfesor);
                foreach (var clase in ordenadas)
                {
                    ClasesMostradas.Add(clase);
                }
            }
            CargarAlumnosDeTodasLasClases((List<ClaseDTO>)listaClases);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
        }
    }



    [RelayCommand]
    public async Task Logout()
    {
        App.Current.Services.GetService<LoginDTO>().Token = "";
        App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().LoginViewModel;
    }



    [RelayCommand]
    public async Task EliminarClase()
    {
        if (ClaseSeleccionada == null)
        {
            Console.WriteLine("No hay ninguna clase seleccionada.");
            return;
        }

        try
        {
            string id = ClaseSeleccionada.Id_clase;
            var result = await _httpJsonProvider.DeleteAsync($"{Constants.CLASE_URL}/{id}");

            if (result == false)
            {
                Console.WriteLine("No se puede borrar la clase seleccionada");
            }
            else
            {
                Console.WriteLine("Clase eliminada correctamente");
                ClasesMostradas.Remove(ClaseSeleccionada);
                LoadAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al borrar la clase: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task CargarAlumnosDeTodasLasClases(List<ClaseDTO> clases)
    {
        foreach (var clase in clases)
        {
            try
            {
                IEnumerable<AlumnoDTO> alumnosEnumerable = await _httpJsonProviderAlumnos.GetAsync(
                    $"{Constants.CLASE_URL}/{clase.Id_clase}/alumnos");

                _alumnosContados = alumnosEnumerable?.Count() ?? 0;

                clase.AlumnosContados = _alumnosContados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar alumnos de la clase {clase.Nombre}: {ex.Message}");
                _alumnosContados = 0;
            }
        }
    }



}

