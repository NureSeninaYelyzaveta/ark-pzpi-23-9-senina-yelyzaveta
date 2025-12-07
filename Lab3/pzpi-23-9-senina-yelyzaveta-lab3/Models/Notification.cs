using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Notification")]
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int? StudentID { get; set; }
        public Student? Student { get; set; }

        public int? ParentID { get; set; }
        public Parent? Parent { get; set; }

        public int? TeacherID { get; set; }
        public Teacher? Teacher { get; set; }
    }
}