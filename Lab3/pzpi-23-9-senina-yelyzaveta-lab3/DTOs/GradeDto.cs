using System;

namespace WebApplicationArtProgress1.DTOs
{
    public class GradeDto
    {
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public int DisciplineID { get; set; }
    }
}