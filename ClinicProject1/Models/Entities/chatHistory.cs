using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.Entities
{
    public class chatHistory
    {
        [Key]
        public int id { get; set; }

        public string targetUser { get; set; }

        public string chatMessage { get; set; }

        [Required]
        public string senderUsername { get; set; }
    }
}
