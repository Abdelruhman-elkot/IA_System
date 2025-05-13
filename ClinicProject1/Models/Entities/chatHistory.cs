using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.Entities
{
    public class chatHistory
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string senderUserId { get; set; }

        [Required]
        public string targetUserId { get; set; }

        public string chatMessage { get; set; }
    }
}
