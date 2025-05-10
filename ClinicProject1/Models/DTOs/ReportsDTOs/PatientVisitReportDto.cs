namespace ClinicProject1.Models.DTOs.ReportsDTOs
{
    public class PatientVisitReportDto
    {
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int TotalVisits { get; set; }
        public string LastVisitDate { get; set; }
        public string MostCommonComplaint { get; set; }
    }
}
