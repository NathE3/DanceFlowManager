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
        private ObservableCollection<ProfesorDTO> _profesores;

        private string _profesorId;
        private InformacionViewModel _informacionViewModel;
        private readonly IHttpJsonProvider<ProfesorDTO> _httpJsonProvider;
        private readonly IClaseServiceToApi _claseServiceToApi;
        private readonly IFileService<ProfesorDTO> _fileService;

        [ObservableProperty]
        private ProfesorDTO _Profesor;

        public DetallesViewModel(IClaseServiceToApi proyectoServiceToApi, IHttpJsonProvider<ProfesorDTO> httpJsonProvider, IFileService<ProfesorDTO> fileService)
        {
            _httpJsonProvider = httpJsonProvider;
            _claseServiceToApi = proyectoServiceToApi;
            _fileService = fileService;
            _profesores = new ObservableCollection<ProfesorDTO>();
        }

        public void SetIdProfesor(string id)
        {
            _profesorId = id;
        }

        public override async Task LoadAsync()
        {
            IEnumerable<ProfesorDTO> profesores = await _httpJsonProvider.GetAsync(Constants.CLASE_URL);
            foreach (var profesor in profesores)
            {
               
                Profesores.Add(profesor);
            }
            Profesor = profesores.FirstOrDefault(x => x.Id == _profesorId) ?? new ProfesorDTO();
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
            Profesor.Estado = "Activo";
            await _claseServiceToApi.CambiarEstado(Profesor);
        }

        [RelayCommand]
        public async Task Denegar()
        {
            Profesor.Estado = "De Baja";
            await _claseServiceToApi.CambiarEstado(Profesor);
        }

    }
}
