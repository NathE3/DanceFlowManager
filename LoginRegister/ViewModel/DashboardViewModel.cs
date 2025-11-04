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

        Clases = new List<ClaseDTO>(); 
        PagedProyectos = new ObservableCollection<ClaseDTO>();

        ItemsPerPage = 5; 
        CurrentPage = 0; 
    }

    private readonly List<ClaseDTO> Clases; 

    [ObservableProperty]
    private ObservableCollection<ClaseDTO> pagedProyectos;

    [ObservableProperty]
    private int currentPage; 

    [ObservableProperty]
    private int itemsPerPage; 

    public int TotalPages => (int)Math.Ceiling((double)Clases.Count / ItemsPerPage);

 
    public override async Task LoadAsync()
    {
        try
        {

            Clases.Clear();
            PagedProyectos.Clear();

          
            IEnumerable<ClaseDTO> listaProyectos= await _httpJsonProvider.GetAsync(Constants.BASE_URL + "");
            Clases.AddRange(listaProyectos.OrderBy(d => d.Id_Profesor));

          
            CurrentPage = 0;
            UpdatePagedProyectos();
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
        }
    }

   
    private void UpdatePagedProyectos()
    {
       
        PagedProyectos.Clear();

        var pagedItems = Clases.Skip(CurrentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
        foreach (var item in pagedItems)
        {
            PagedProyectos.Add(item);
        }
    }

    [RelayCommand]
    public async Task Logout() 
    {
        App.Current.Services.GetService<LoginDTO>().Token = "";
        App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().LoginViewModel;
    }

    [RelayCommand]
    public void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            UpdatePagedProyectos();
        }
    }

    [RelayCommand]
    public void NextPage()
    {
        if (CurrentPage < TotalPages - 1)
        {
            CurrentPage++;
            UpdatePagedProyectos();
        }
    }
    public async void  MyDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Row.Item is ClaseDTO proyectoDTO)
        {
           await _proyectoServiceToApi.PutProyecto(proyectoDTO);
        }
    }
    private bool CanGoToPreviousPage() => CurrentPage > 0;

    private bool CanGoToNextPage() => CurrentPage < TotalPages - 1;
}

