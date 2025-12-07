using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Admin")]
    public class Admin
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
 