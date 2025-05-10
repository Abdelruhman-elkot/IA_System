namespace ClinicProject1.Models.DTOs.DoctorDTOs
{
    public class DoctorDashboardDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public List<string> AvailableDays { get; set; } = new List<string>();
        public string Availability { get; set; }
    }
}
