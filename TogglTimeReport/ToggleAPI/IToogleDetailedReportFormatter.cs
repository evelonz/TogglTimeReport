
namespace TogglTimeReport.ToggleAPI
{
    public interface IToogleDetailedReportFormatter
    {
        void Append(DetailedReportResponse report);

        string GetFormattedReport();
    }
}
