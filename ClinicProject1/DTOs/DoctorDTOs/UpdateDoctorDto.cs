using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.DTOs.Doctor_DTOs
{
    public class UpdateDoctorDto
    {
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
