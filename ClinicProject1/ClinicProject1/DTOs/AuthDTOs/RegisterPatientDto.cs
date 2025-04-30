using ClinicProject1.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.DTOs.AuthDTOs
{
    public class RegisterPatientDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public string ChronicDiseases { get; set; }
        public string MedicalComplaint { get; set; }
    }
}
