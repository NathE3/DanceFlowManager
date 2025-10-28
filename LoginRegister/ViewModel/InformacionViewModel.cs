using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Interface;
using InfoManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoManager.ViewModel
{
    public partial class InformacionViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ProyectoDTO> items;


        private readonly IProyectoServiceToApi _dicatadorServiceToApi;
        private readonly DetallesViewModel _detallesViewModel;
        private readonly IStringUtils _stringUtils;

        [ObservableProperty]
        private ViewModelBase? _selectedViewModel;

        public InformacionViewModel(IProyectoServiceToApi dicatadorServiceToApi, DetallesViewModel detallesViewModel, IStringUtils stringUtils)
        {
            _dicatadorServiceToApi = dicatadorServiceToApi;
            _detallesViewModel = detallesViewModel;
            _stringUtils = stringUtils;
            items = new ObservableCollection<ProyectoDTO>();
        }

        public override async Task LoadAsync()
        {
            Items.Clear();
            IEnumerable<ProyectoDTO> dicatatores = await _dicatadorServiceToApi.GetProyectos();
            foreach (var dicatador in dicatatores)
            {
                
                items.Add(dicatador);
            }
        }

        [RelayCommand]
        private async Task SelectViewModel(object? parameter)
        {
            _detallesViewModel.SetIdDicatador(_stringUtils.ConvertToInteger(parameter?.ToString() ?? string.Empty) ?? int.MinValue);
            _detallesViewModel.SetParentViewModel(this);
            SelectedViewModel = _detallesViewModel;
            await _detallesViewModel.LoadAsync();
        }
    }
}
