using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Discipline")]
    public class Discipline
    {
        [Key]
        public int DisciplineID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
        public ICollection<Grade> Grades { get; set; }

    }
}

