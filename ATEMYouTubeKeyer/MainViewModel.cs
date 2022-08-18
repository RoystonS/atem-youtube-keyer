using System.Collections.Generic;

namespace ATEMYouTubeKeyer
{
    public class MainViewModel : ViewModelBase
    {
        public async void RetrieveDataFromYouTube()
        {
            var liveBroadcasts = await YouTube.GetLiveBroadcastsAsync();
            this.LiveBroadcasts = liveBroadcasts;
        }

        private List<LiveBroadcastInfo> liveBroadcasts = new();
        public List<LiveBroadcastInfo> LiveBroadcasts
        {
            get { return liveBroadcasts; }
            private set
            {
                SetProperty(ref liveBroadcasts, value);
            }
        }
    }
}
