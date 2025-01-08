using SQLite;

namespace People.Models;

[Table("people")]
public class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(125)]
    public string Nombre { get; set; }
    public string Apellido { get; set; }
}
