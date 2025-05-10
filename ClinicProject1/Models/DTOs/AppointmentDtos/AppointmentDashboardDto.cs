namespace ClinicProject1.Models.DTOs.AppointmentDtos
{
    public class AppointmentDashboardDto
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string AppointmentDay { get; set; }

        public string AppointmentTime { get; set; }

        public string Status { get; set; }

        public int DoctorId { get; set; }
    }
}
