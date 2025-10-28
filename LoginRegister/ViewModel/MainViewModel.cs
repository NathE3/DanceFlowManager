using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;


namespace InfoManager.ViewModel;

public partial class MainViewModel : ViewModelBase
{
    private ViewModelBase? _selectedViewModel;
    private bool _isMenuEnabled;

    public MainViewModel(DetallesViewModel detallesViewModel, InformacionViewModel informacionViewModel, DashboardViewModel dashboardViewModel, LoginViewModel loginViewModel, RegistroViewModel registroViewModel)
    {
        DashboardViewModel = dashboardViewModel;
        LoginViewModel = loginViewModel;
        RegistroViewModel = registroViewModel;
        InformacionViewModel = informacionViewModel;
        DetallesViewModel = detallesViewModel;
        _selectedViewModel = loginViewModel;
    }

    public ViewModelBase? SelectedViewModel
    {
        get => _selectedViewModel;
        set
        {
            SetProperty(ref _selectedViewModel, value);
            if (value is LoginViewModel)
            {
                IsMenuEnabled = false;
                
            }
            else
            {
                IsMenuEnabled = true;
                
            }
        }
    }

    public DashboardViewModel DashboardViewModel { get; }
    public LoginViewModel LoginViewModel { get; }
    public InformacionViewModel InformacionViewModel { get; }
    public DetallesViewModel DetallesViewModel { get; }
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




