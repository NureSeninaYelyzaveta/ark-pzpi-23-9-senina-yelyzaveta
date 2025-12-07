using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Grade")]
    public class Grade
    {
        public int GradeID { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public int DisciplineID { get; set; }


        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Discipline Discipline { get; set; }

    }
}
