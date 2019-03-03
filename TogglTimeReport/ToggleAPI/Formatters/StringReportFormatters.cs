using System;
using System.Linq;
using System.Text;

namespace TogglTimeReport.ToggleAPI.Formatters
{
    public abstract class BaseStringReport : IToogleDetailedReportFormatter
    {
        protected StringBuilder SB { get; set; }

        public BaseStringReport()
        {
            SB = new StringBuilder();
        }

        public abstract void Append(DetailedReportResponse report);

        public string GetFormattedReport()
        {
            return SB.ToString();
        }

        protected string GetDecimalHours(int milliseconds)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds).TotalHours.ToString("0.##");
        }
    }

    public class ProjectGroupedTimeReport : BaseStringReport
    {
        public override void Append(DetailedReportResponse report)
        {
            var data = report.data;
            var groupedData = data.GroupBy(g => new { Date = g.start.Date, Project = g.project, User = g.user });

            foreach (var item in groupedData)
            {
                // Project (system, part of the system, CRnr, workitems)	Who	When	Hours	Tag (buggfix, new feature, meeting, update, support)
                this.SB.Append($"{item.Key.Project};{item.Key.User};{item.Key.Date:yyyy-MM-dd};{GetDecimalHours(item.Sum(s => s.dur))};{String.Join(", ", item.Select(s => s.description))}{Environment.NewLine}");
            }
        }
    }

    public class TaskGroupedTimeReport : BaseStringReport
    {
        public override void Append(DetailedReportResponse report)
        {
            var data = report.data;
            var groupedData = data.GroupBy(g => new { Date = g.start.Date, Project = g.project, Task = g.description, User = g.user });

            foreach (var item in groupedData)
            {
                // Project (system, part of the system, CRnr, workitems)	Who	When	Hours	Tag (buggfix, new feature, meeting, update, support)
                this.SB.Append($"{item.Key.Date:yyyy-MM-dd};{item.Key.Project};{item.Key.Task};{GetDecimalHours(item.Sum(s => s.dur))}{Environment.NewLine}");
            }
        }
    }
}
