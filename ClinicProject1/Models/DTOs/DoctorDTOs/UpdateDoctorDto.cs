using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.DTOs.DoctorDTOs
{
    public class UpdateDoctorDto
    {
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
