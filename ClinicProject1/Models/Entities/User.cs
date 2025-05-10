using ClinicProject1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.Entities
{
    public class User
    {
        public User()
        {
            Photo = "default-profile.png";
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        public string? Photo { get; set; }

        [Required]
        [StringLength(255)]
        [MaxLength(12)]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
