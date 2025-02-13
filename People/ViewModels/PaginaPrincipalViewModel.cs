﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels
{
    public class mathiasveraPaginaPrincipalViewModel : BindableObject
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

        public mathiasveraPaginaPrincipalViewModel()
        {
            // Inicializar repositorio con la ruta de la base de datos con mi nombre
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MathiasVera.db");
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

                StatusMessage = "Personas recuperadas con éxito.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al recuperar personas: {ex.Message}";
            }
        }

        private void DeletePerson(Person person)
        {
            try
            {
                if (person == null) return;

                // Eliminar persona de la base de datos
                _personRepository.DeletePerson(person.Id);
                StatusMessage = $"{person.Nombre} ha sido eliminado por Mathias Vera";

                // Actualizar lista
                GetAllPeople();

                // Mostrar alerta
                App.Current.MainPage.DisplayAlert("Registro eliminado", $"Mathias Vera acaba de eliminar a {person.Nombre}", "OK");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Mathias Vera, error eliminando a persona: {ex.Message}";
            }
        }
    }
}
