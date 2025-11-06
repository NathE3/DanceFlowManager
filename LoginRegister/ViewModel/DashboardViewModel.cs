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

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IHttpJsonProvider<ClaseDTO> _httpJsonProvider;

    public DashboardViewModel(IHttpJsonProvider<ClaseDTO> httpJsonProvider)
    {
        _httpJsonProvider = httpJsonProvider;


        Clases = [];
        _clasesMostradas = [];

    }

    private readonly List<ClaseDTO> Clases; 

    [ObservableProperty]
    private readonly ObservableCollection<ClaseDTO> _clasesMostradas;


 
    public override async Task LoadAsync()
    {
        try
        {

            Clases.Clear();

          
            IEnumerable<ClaseDTO> listaClases= await _httpJsonProvider.GetAsync(Constants.BASE_URL + Constants.CLASE_URL);
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

}

