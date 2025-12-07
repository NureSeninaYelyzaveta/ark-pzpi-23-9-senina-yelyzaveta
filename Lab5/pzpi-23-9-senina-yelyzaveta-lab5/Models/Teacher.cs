using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models;

    [Table("Teacher")]
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }

    // зв'язки з іншими таблицями
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
