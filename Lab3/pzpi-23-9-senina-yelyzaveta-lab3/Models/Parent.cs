using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationArtProgress1.Models
{
    [Table("Parent")]
    public class Parent
    {
        [Key]
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public ICollection<ParentStudent> ParentStudents { get; set; } //через проміжну табл
        public ICollection<Notification> Notifications { get; set; }

    }
}
