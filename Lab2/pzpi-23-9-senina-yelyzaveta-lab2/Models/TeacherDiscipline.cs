using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplicationArtProgress1.Models;


[Table("TeacherDiscipline")]
public class TeacherDiscipline
{
    public int TeacherID { get; set; }
    public int DisciplineID { get; set; }

    [JsonIgnore]
    public Teacher? Teacher { get; set; }

    [JsonIgnore]
    public Discipline? Discipline { get; set; }
}
