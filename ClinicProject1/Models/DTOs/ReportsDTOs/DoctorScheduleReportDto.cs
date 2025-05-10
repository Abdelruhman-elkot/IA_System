namespace ClinicProject1.Models.DTOs.ReportsDTOs
{
    public class DoctorScheduleReportDto
    {
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public List<string> AvailableDays { get; set; }
        public string WorkingHours { get; set; }
        public int TotalAppointments { get; set; }
    }
}
