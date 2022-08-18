using Google.Apis.YouTube.v3.Data;
using System;
using System.Globalization;

namespace ATEMYouTubeKeyer
{
    public class LiveBroadcastInfo
    {
        public LiveBroadcastInfo(string title, DateTime? start, DateTime? end, IngestionInfo ingestionInfo)
        {
            Title = title;
            Start = start;
            End = end;
            IngestionInfo = ingestionInfo;
        }

        public string Title { get; }
        public DateTime? Start { get; }
        public string StartString
        {
            get { return Start.HasValue ? Start.Value.ToString("f", DateTimeFormatInfo.CurrentInfo) : ""; }
        }
        public DateTime? End { get; }
        public IngestionInfo IngestionInfo { get; }
    }
}
