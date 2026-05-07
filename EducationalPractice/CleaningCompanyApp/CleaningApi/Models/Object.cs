using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Models
{
    public class CleaningObject
    {
        [Key]
        public int Id_Object { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public double Area { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
    }
}
