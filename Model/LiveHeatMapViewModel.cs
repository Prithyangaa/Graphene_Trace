using System;
using System.Collections.Generic;

namespace GrapheneTrace.Models
{
    public class LiveHeatMapViewModel
    {
        public string Title { get; set; } = "Live Heat Map";
        public double PeakPressure { get; set; } = 85.0; // mmHg
        public double ContactArea { get; set; } = 72.0;  // %
        public double AveragePressure { get; set; } = 42.0; // mmHg
        public string Status { get; set; } = "Normal";

        public List<ActivityItem> RecentActivities { get; set; } = new()
        {
            new ActivityItem { Description = "Pressure redistributed", TimeAgo = "2 mins ago" },
            new ActivityItem { Description = "Patient repositioned", TimeAgo = "15 mins ago" },
            new ActivityItem { Description = "Alert threshold adjusted", TimeAgo = "1 hour ago" }
        };
    }

    public class ActivityItem
    {
        public string Description { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
    }
}
