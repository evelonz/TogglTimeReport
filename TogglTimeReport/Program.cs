using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TogglTimeReport.ToggleAPI;
using TogglTimeReport.ToggleAPI.Formatters;

namespace TogglTimeReport
{
    class Program
    {
        private static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            // Set for using the Toggle API.
            // API Token is found under your Profile settings page (https://toggl.com/app/profile).
            var token = "___";
            // The name of your application or your email address so we can get in touch in case you're doing something wrong.
            var userAgent = "__@{YourDomain.com}";
            // The workspace whose data you want to access. Can usually be found in the URL, for instance when viewing reports.
            var workspaceID = "1234567";

            // The dates for the report (inclusive).
            var fromDate = "2019-02-23";
            var toDate = "2019-03-01";

            var formatters = new IToogleDetailedReportFormatter[]
            {
                new ProjectGroupedTimeReport(),
                new TaskGroupedTimeReport(),
            };

            var reader = new ToggleDetailedReport(token, userAgent, workspaceID, formatters);
            
            await reader.GetDetailedReportAsCsv(fromDate, toDate);

            WriteReport(fromDate, toDate, formatters);

            Console.WriteLine($"{Environment.NewLine}END OF REPORTS! {Environment.NewLine}Press key to end.");

            Console.ReadKey();
        }

        private static void WriteReport(string fromDate, string toDate, IToogleDetailedReportFormatter[] formatters)
        {
            var folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            foreach (var formatter in formatters)
            {
                Console.WriteLine($"Report of type: {formatter.GetType().Name}{Environment.NewLine}");

                var fileName = $"{formatter.GetType().Name}_{fromDate:yyyy-MM-dd}_{toDate:yyyy-MM-dd}.txt";
                var path = System.IO.Path.Combine(folder, fileName);
                Console.WriteLine($"Writing report to: {path}");
                try
                {
                    System.IO.File.WriteAllText(path, formatter.GetFormattedReport());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Filed writing report! {ex.Message}");
                }
            }
        }

    }
    
}
