namespace GrapheneTrace.Model
{
    public class ClinicianHeatmapViewModel
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string CsvUrl { get; set; } = string.Empty;

        public int TotalFrames { get; set; }
        public int FrameIntervalMinutes { get; set; } = 5;
    }
}
