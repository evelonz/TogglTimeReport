using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TogglTimeReport.ToggleAPI
{
    public class ToggleDetailedReport
    {

        private static HttpClient client = new HttpClient();
        private static readonly string baseUrl = "https://toggl.com/reports/api/v2";
        private static readonly int MinMsDelayBetweenRequests = 1002;

        private readonly string userAgent = "";
        private readonly string workspaceID = "";
        private IToogleDetailedReportFormatter[] formatters;

        public ToggleDetailedReport(string ApiToken, string userAgent, string workspaceID, params IToogleDetailedReportFormatter[] formatters)
        {
            this.userAgent = userAgent;
            this.workspaceID = workspaceID;
            this.formatters = formatters;

            client.BaseAddress = new Uri(baseUrl); // error500
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var cred = Base64Encode($"{ApiToken}:api_token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", cred);
        }

        public void SetFormatters(params IToogleDetailedReportFormatter[] formatters)
        {
            this.formatters = formatters;
        }

        public async Task GetDetailedReportAsCsv(string fromDate, string toDate)
        {
            var url = CreateBaseGetRequest(fromDate, toDate);
            await GetFullReportAsCsv(url);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private string CreateBaseGetRequest(string fromDate, string toDate)
        {
            var builder = new UriBuilder(baseUrl + "/details");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["user_agent"] = userAgent;
            query["workspace_id"] = workspaceID;

            query["since"] = fromDate;
            query["until"] = toDate;
            // page (defualt 1). Unique for Detailed Report.

            builder.Query = query.ToString();
            return builder.ToString();
        }


        private async Task GetFullReportAsCsv(string url)
        {
            var sw = new Stopwatch(); // Try and extract this to an aspect, httphandler, or something else (preferably thread safe)
            var CurrentPage = 1;

            while (true)
            {
                var urlWithPage = url + $"&page={CurrentPage++}";
                sw.Restart();
                HttpResponseMessage response = await client.GetAsync(urlWithPage);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var reportResponse = JsonConvert.DeserializeObject<DetailedReportResponse>(content);

                foreach (var formatter in formatters)
                {
                    formatter.Append(reportResponse);
                }

                if (reportResponse.data.Count() < reportResponse.per_page)
                {
                    break;
                }

                var msPassed = sw.ElapsedMilliseconds;
                if (msPassed < MinMsDelayBetweenRequests)
                {
                    Thread.Sleep(MinMsDelayBetweenRequests - (int)msPassed);
                }
            }

        }

    }
}
