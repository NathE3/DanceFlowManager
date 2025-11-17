using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;


namespace InfoManager.ViewModel
{
    public partial class InformacionViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<AlumnoDTO> items;

        private readonly IHttpJsonProvider<AlumnoDTO> _httpJsonProvider;

        [ObservableProperty]
        private ViewModelBase? _selectedViewModel;

        public InformacionViewModel(IHttpJsonProvider<AlumnoDTO> httpJsonProvider, DetallesViewModel detallesViewModel, IStringUtils stringUtils)
        {
            _httpJsonProvider = httpJsonProvider;
            items = [];
        }

        public override async Task LoadAsync()
        {
            Items.Clear();
            IEnumerable<AlumnoDTO> alumnos = await _httpJsonProvider.GetAsync(Constants.ALUMNO_URL);
            var alumnosOrdenados = alumnos.OrderBy(a => a.Apellidos).ThenBy(a => a.Name);
            foreach (var alumno in alumnos)
            {

                items.Add(alumno);
            }

        }
    }
}
