using System;
using System.Collections.Generic;
using System.Text;

namespace TogglTimeReport.ToggleAPI
{
    // Not used yet.
    class ToggleReportApiParameters
    {
        //user_agent: Required.The name of your application or your email address so we can get in touch in case you're doing something wrong.
        public string user_agent { get; set; }
        //workspace_id: Required.The workspace whose data you want to access.
        public string workspace_id { get; set; }
        //since: ISO 8601 date (YYYY-MM-DD) format.Defaults to today - 6 days.
        public DateTime? since { get; set; }
        //until: ISO 8601 date(YYYY-MM-DD) format.Note: Maximum date span(until - since) is one year.Defaults to today, unless since is in future or more than year ago, in this case until is since + 6 days.
        public DateTime? until { get; set; }
        //billable: "yes", "no", or "both". Defaults to "both".
        //client_ids: A list of client IDs separated by a comma.Use "0" if you want to filter out time entries without a client.
        //project_ids: A list of project IDs separated by a comma.Use "0" if you want to filter out time entries without a project.
        //user_ids: A list of user IDs separated by a comma.
        //members_of_group_ids: A list of group IDs separated by a comma.This limits provided user_ids to the members of the given groups.
        //or_members_of_group_ids: A list of group IDs separated by a comma.This extends provided user_ids with the members of the given groups.
        //tag_ids: A list of tag IDs separated by a comma.Use "0" if you want to filter out time entries without a tag.
        //task_ids: A list of task IDs separated by a comma.Use "0" if you want to filter out time entries without a task.
        //time_entry_ids: A list of time entry IDs separated by a comma.
        //description: Matches against time entry descriptions.
        //without_description: "true" or "false". Filters out the time entries which do not have a description (literally "(no description)").
        //order_field:
        //For detailed reports: "date", "description", "duration", or "user"
        //For summary reports: "title", "duration", or "amount"
        //For weekly reports: "title", "day1", "day2", "day3", "day4", "day5", "day6", "day7", or "week_total"
        //order_desc: "on" for descending, or "off" for ascending order.
        //distinct_rates: "on" or "off". Defaults to "off".
        //rounding: "on" or "off". Defaults to "off". Rounds time according to workspace settings.
        public bool rounding { get; set; }
        //display_hours: "decimal" or "minutes". Defaults to "minutes". Determines whether to display hours as a decimal number or with minutes.
        public bool display_hours { get; set; }
    }
}
