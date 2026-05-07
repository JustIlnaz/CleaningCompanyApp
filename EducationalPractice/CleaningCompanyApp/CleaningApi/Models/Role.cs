using System.ComponentModel.DataAnnotations;

namespace CleaningApi.Models
{
    public class Role
    {
        [Key]
        public int Id_Role { get; set; }
        public string Name { get; set; }
    }
}
