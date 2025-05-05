using ClinicProject1.Data.Enums;
using ClinicProject1.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.DTOs.Doctor_DTOs
{
    public class CreateDoctorDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Specialization Specialization { get; set; }
    }
}
