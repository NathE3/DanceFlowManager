using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.View;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InfoManager.ViewModel;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IProyectoServiceToApi _proyectoServiceToApi;

    public DashboardViewModel(IProyectoServiceToApi proyectoServiceToApi)
    {
        _proyectoServiceToApi = proyectoServiceToApi;

        Proyectos = new List<ProyectoDTO>(); 
        PagedProyectos = new ObservableCollection<ProyectoDTO>();

        ItemsPerPage = 5; 
        CurrentPage = 0; 
    }

    private List<ProyectoDTO> Proyectos; 

    [ObservableProperty]
    private ObservableCollection<ProyectoDTO> pagedProyectos;

    [ObservableProperty]
    private int currentPage; 

    [ObservableProperty]
    private int itemsPerPage; 

    public int TotalPages => (int)Math.Ceiling((double)Proyectos.Count / ItemsPerPage);

 
    public override async Task LoadAsync()
    {
        try
        {
            
            Proyectos.Clear();
            PagedProyectos.Clear();

          
            IEnumerable<ProyectoDTO> listaProyectos= await _proyectoServiceToApi.GetProyectos();
            Proyectos.AddRange(listaProyectos.OrderBy(d => d.Id));

          
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

        var pagedItems = Proyectos.Skip(CurrentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
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
        if (e.Row.Item is ProyectoDTO proyectoDTO)
        {
           await _proyectoServiceToApi.PutProyecto(proyectoDTO);
        }
    }
    private bool CanGoToPreviousPage() => CurrentPage > 0;

    private bool CanGoToNextPage() => CurrentPage < TotalPages - 1;
}

