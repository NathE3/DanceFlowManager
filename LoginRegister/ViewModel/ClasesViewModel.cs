using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.View;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows.Controls;

namespace InfoManager.ViewModel;

public partial class ClasesViewModel : ViewModelBase
{
    private readonly IHttpJsonProvider<ClaseDTO> _httpJsonProvider;

    private readonly List<ClaseDTO> Clases;
    private ClaseDTO _claseSeleccionada;

    [ObservableProperty]
    private ObservableCollection<ClaseDTO> _clasesMostradas;
    public ClasesViewModel(IHttpJsonProvider<ClaseDTO> httpJsonProvider)
    {
        _httpJsonProvider = httpJsonProvider;
        Clases = [];
        _clasesMostradas = [];

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

            Clases.Clear();


            IEnumerable<ClaseDTO> listaClases = await _httpJsonProvider.GetAsync(Constants.BASE_URL + Constants.CLASE_URL);
            Clases.AddRange(listaClases.OrderBy(d => d.Id_Profesor));

            foreach (ClaseDTO clase in Clases)
            {
                _clasesMostradas.Add(clase);
            }
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al borrar la clase: {ex.Message}");
        }
    }


}

