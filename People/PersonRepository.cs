using SQLite;
using People.Models;


namespace People;

public class PersonRepository
{
    string _dbPath;

    public string StatusMessage { get; set; }

    private SQLiteConnection conn;

    private void Init()
    {
        if (conn != null)
            return;

        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;                        
    }

    public void AddNewPerson(string name)
    {            
        int result = 0;
        try
        {
            // TODO: Call Init()
            Init();

            // basic validation to ensure a name was entered
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            // TODO: Insert the new person into the database
            result = conn.Insert(new Person { Nombre = name });

            StatusMessage = string.Format("Mathias Vera: {0} record(s) agregado (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Mathias Vera: fallo al agregar {0}. Error: {1}", name, ex.Message);
        }

    }

    public List<Person> GetAllPeople()
    {
        // TODO: Init then retrieve a list of Person objects from the database into a list
        try
        {
            Init ();
            return conn.Table<Person>().ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public void DeletePerson(int personId)
    {
        try
        {
            Init();
            var personToDelete = conn.Find<Person>(personId);

            if (personToDelete != null)
            {
                conn.Delete(personToDelete);
                StatusMessage = $"Record with ID {personId} deleted.";
            }
            else
            {
                StatusMessage = $"No record found with ID {personId}.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to delete person. Error: {ex.Message}";
        }
    }
}
