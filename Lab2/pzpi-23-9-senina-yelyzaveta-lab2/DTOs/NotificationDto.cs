using System;

namespace WebApplicationArtProgress1.DTOs
{
    public class NotificationDto
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int StudentID { get; set; }
        public int ParentID { get; set; }
        public int TeacherID { get; set; }
    }
}