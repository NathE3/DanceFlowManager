using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InfoManager.ViewModel
{
    public partial class DetallesViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ObservableCollection<ProyectoDTO> _items;

        private int _dicatadorId;
        private InformacionViewModel _informacionViewModel;
        private readonly IHttpJsonProvider<ProyectoDTO> _httpJsonProvider;
        private readonly IProyectoServiceToApi _proyectoServiceToApi;
        private readonly IFileService<ProyectoDTO> _fileService;

        [ObservableProperty]
        private ProyectoDTO _Dicatador;

        public DetallesViewModel(IProyectoServiceToApi proyectoServiceToApi, IHttpJsonProvider<ProyectoDTO> httpJsonProvider, IFileService<ProyectoDTO> fileService)
        {
            _httpJsonProvider = httpJsonProvider;
            _proyectoServiceToApi = proyectoServiceToApi;
            _fileService = fileService;
            _items = new ObservableCollection<ProyectoDTO>();
        }

        public void SetIdDicatador(int id)
        {
            _dicatadorId = id;
        }

        public override async Task LoadAsync()
        {
            IEnumerable<ProyectoDTO> dicatadores = await _httpJsonProvider.GetAsync(Constants.PROYECTO_URL);
            foreach (var dicatador in dicatadores)
            {
               
                Items.Add(dicatador);
            }
            Dicatador = dicatadores.FirstOrDefault(x => x.Id == _dicatadorId) ?? new ProyectoDTO();
        }

        internal void SetParentViewModel(ViewModelBase informacionViewModel)
        {
            if (informacionViewModel is InformacionViewModel informacionview)
            {
                _informacionViewModel = informacionview;
            }
        }

        [RelayCommand]
        private async Task Close(object? parameter)
        {
            if (_informacionViewModel != null)
            {
                _informacionViewModel.SelectedViewModel = null;
            }
        }

        [RelayCommand]
        public async Task Aprobar()
        {
            Dicatador.Estado = "Aprobado";
            await _proyectoServiceToApi.CambiarEstado(Dicatador);
        }

        [RelayCommand]
        public async Task Denegar()
        {
            Dicatador.Estado = "Denegado";
            await _proyectoServiceToApi.CambiarEstado(Dicatador);
        }

    }
}
