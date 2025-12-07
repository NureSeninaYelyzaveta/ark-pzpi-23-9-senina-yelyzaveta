using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models;

    [Table("Student")]
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string ContactInfo { get; set; }

    // зв'язки з іншими таблицями
    public ICollection<ParentStudent> ParentStudents { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public Portfolio Portfolio { get; set; }
}


