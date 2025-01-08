using People.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace People
{
    public class PaginaPrincipalViewModel
    {
        private SQLiteConnection _database;

        public ObservableCollection<Person> People { get; set; }

        public ICommand DeleteCommand { get; }

        public PaginaPrincipalViewModel()
        {
            // Inicializa la base de datos
            _database = new SQLiteConnection("localdata.db");
            _database.CreateTable<Person>();

            People = new ObservableCollection<Person>(_database.Table<Person>().ToList());

            DeleteCommand = new Command<Person>(DeletePerson);
        }

        private void DeletePerson(Person person)
        {
            // Elimina la persona de la base de datos
            _database.Delete(person);
            People.Remove(person);

            // Muestra la alerta
            App.Current.MainPage.DisplayAlert("Registro eliminado", $"{person.Nombre} {person.Apellido} acaba de eliminar un registro.", "OK");
        }
    }
}
