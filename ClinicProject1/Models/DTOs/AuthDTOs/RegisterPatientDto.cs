using ClinicProject1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.DTOs.AuthDTOs
{
    public class RegisterPatientDto
    {
        //public string Username { get; set; } = FirstName 
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
