namespace ClinicProject1.Services.Interfaces
{
    public interface IPdfService
    {
        byte[] GeneratePdf(string htmlContent);
    }
}
