using System;

namespace TogglTimeReport.ToggleAPI
{
    public class DetailedReportResponse
    {
        public int total_grand { get; set; }
        public int? total_billable { get; set; }
        public int total_count { get; set; }
        public int per_page { get; set; }
        public Total_Currencies[] total_currencies { get; set; }
        public DataEntries[] data { get; set; }
    }

    public class Total_Currencies
    {
        public string currency { get; set; }
        public float? amount { get; set; }
    }

    public class DataEntries
    {
        public int id { get; set; }
        public int? pid { get; set; }
        public int? tid { get; set; }
        public int uid { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime updated { get; set; }
        public int dur { get; set; }
        public string user { get; set; }
        public bool use_stop { get; set; }
        public string client { get; set; }
        public string project { get; set; }
        public string task { get; set; }
        public float? billable { get; set; }
        public bool is_billable { get; set; }
        public string cur { get; set; }
        public string[] tags { get; set; }
    }
}
