using System.Collections.ObjectModel;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private readonly PersonRepository _personRepository;

        private string _newPersonName;
        public string NewPersonName
        {
            get => _newPersonName;
            set
            {
                _newPersonName = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> People { get; set; }

        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public MainPageViewModel()
        {
            // Inicializar repositorio con la ruta de la base de datos
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "localdata.db");
            _personRepository = new PersonRepository(dbPath);

            People = new ObservableCollection<Person>();

            // Comandos
            AddPersonCommand = new Command(AddPerson);
            GetPeopleCommand = new Command(GetAllPeople);
            DeletePersonCommand = new Command<Person>(DeletePerson);

            // Cargar personas al iniciar
            GetAllPeople();
        }

        private void AddPerson()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewPersonName))
                {
                    StatusMessage = "Name cannot be empty.";
                    return;
                }

                _personRepository.AddNewPerson(NewPersonName);
                StatusMessage = _personRepository.StatusMessage;

                // Actualizar lista
                GetAllPeople();

                // Limpiar entrada
                NewPersonName = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding person: {ex.Message}";
            }
        }

        private void GetAllPeople()
        {
            try
            {
                var peopleFromDb = _personRepository.GetAllPeople();
                People.Clear();

                foreach (var person in peopleFromDb)
                {
                    People.Add(person);
                }

                StatusMessage = "People retrieved successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error retrieving people: {ex.Message}";
            }
        }

        private void DeletePerson(Person person)
        {
            try
            {
                if (person == null) return;

                // Eliminar persona de la base de datos
                _personRepository.DeletePerson(person.Id);
                StatusMessage = $"{person.Nombre} ha sido eliminado.";

                // Actualizar lista
                GetAllPeople();

                // Mostrar alerta
                App.Current.MainPage.DisplayAlert("Registro eliminado", $"{person.Nombre} ha sido eliminado.", "OK");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting person: {ex.Message}";
            }
        }
    }
}
