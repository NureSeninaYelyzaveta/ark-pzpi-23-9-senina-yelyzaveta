using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace WebApplicationArtProgress1.Models;
 
[Table("ParentStudent")]
    public class ParentStudent
    {
        public int ParentID { get; set; }
        public int StudentID { get; set; }

        [JsonIgnore]
        public Parent? Parent { get; set; }

        [JsonIgnore]
        public Student? Student { get; set; }
    }
