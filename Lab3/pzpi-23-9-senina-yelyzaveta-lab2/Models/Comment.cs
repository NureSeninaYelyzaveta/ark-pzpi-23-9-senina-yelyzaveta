using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Comment")]
    public class Comment
    {
        public int CommentID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }


        public Student Student { get; set; }
        public Teacher Teacher { get; set; }

    }
}
