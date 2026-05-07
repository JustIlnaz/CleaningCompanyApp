using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApi.Models
{
    public class Session
    {
        [Key]
        public int Id_Session { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
