namespace ClinicProject1.Models.Enums
{
    public enum Role
    {
        Admin,
        Doctor,
        Patient
    }

    public enum Specialization
    {
        Dentist,
        Cardiologist,
        Neurologist,
        Pediatrician,
        Dermatologist,
        Orthopedic,
        Psychiatrist,
        Ophthalmologist,
        Gynecologist
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum AppointmentStatus
    {
        Pending,
        Approved,
        Completed,
        Canceled
    }

    public enum WorkDays
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Saturday
    }

    public enum AppointmentTimes
    {
        Time_0500,
        Time_0530,
        Time_0600,
        Time_0630,
        Time_0700,
        Time_0730,
        Time_0800,
        Time_0830,
        Time_0900,
        Time_0930
    }

    public enum ReportType
    {
        DoctorSchedules,
        PatientVisits
    }
}