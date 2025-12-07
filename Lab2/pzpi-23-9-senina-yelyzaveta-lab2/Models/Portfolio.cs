using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Portfolio")]
    public class Portfolio
    {
        public int PortfolioID { get; set; }
        public string Files { get; set; }
        public int StudentID { get; set; }

        // осилання на студента, якому належить запис
        public Student Student { get; set; }


    }
}