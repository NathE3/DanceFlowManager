using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;


namespace InfoManager.ViewModel;

public partial class MainViewModel : ViewModelBase
{
    private ViewModelBase? _selectedViewModel;
    private bool _isMenuEnabled;

    public MainViewModel(CrearClaseViewModel crearClaseViewModel, ListadoAlumnosViewModel listadoAlumnosViewModel, ClasesViewModel clasesViewModel, LoginViewModel loginViewModel, RegistroViewModel registroViewModel)
    {
        ClasesViewModel = clasesViewModel;
        LoginViewModel = loginViewModel;
        RegistroViewModel = registroViewModel;
        ListadoAlumnosViewModel = listadoAlumnosViewModel;
        CrearClaseViewModel = crearClaseViewModel;
        _selectedViewModel = loginViewModel;
    }

    public ViewModelBase? SelectedViewModel
    {
        get => _selectedViewModel;
        set
        {
            SetProperty(ref _selectedViewModel, value);
            if (value is LoginViewModel || value is RegistroViewModel)
            {
                IsMenuEnabled = false;
                
            }
            else
            {
                IsMenuEnabled = true;
                
            }
        }
    }

    public ClasesViewModel ClasesViewModel { get; }
    public LoginViewModel LoginViewModel { get; }
    public ListadoAlumnosViewModel ListadoAlumnosViewModel { get; }
    public CrearClaseViewModel CrearClaseViewModel { get; }
    public RegistroViewModel RegistroViewModel { get; }


    public override async Task LoadAsync()
    {
        if (SelectedViewModel is not null)
        {
            await SelectedViewModel.LoadAsync();
        }
    }

    [RelayCommand]
    private async Task SelectViewModelAsync(object? parameter)
    {
        if (parameter is ViewModelBase viewModel)
        {
            SelectedViewModel = viewModel;
            await LoadAsync();
        }
    }

    public bool IsMenuEnabled
    {
        get => _isMenuEnabled;
        set => SetProperty(ref _isMenuEnabled, value);
    }



}




