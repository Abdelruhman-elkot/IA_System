using ClinicProject1.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject1.Models.DTOs.AvailabilityDTOs
{
    public class AssignAvailabilityDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "At least two days must be selected")]
        [MaxLength(2, ErrorMessage = "Exactly two days must be selected")]
        public List<WorkDays> AvailableDays { get; set; }
    }
}
