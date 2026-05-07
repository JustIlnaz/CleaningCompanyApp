using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Brigade
    {
        [Key]
        public int Id_Brigade { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }

        [ForeignKey("User")]
        public int? BrigadierId { get; set; }
        public User Brigadier { get; set; }
    }
}
