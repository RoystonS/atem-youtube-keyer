using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ATEMYouTubeKeyer
{
    internal class YouTube
    {
        public static async Task<List<LiveBroadcastInfo>> GetLiveBroadcastsAsync()
        {

            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(nameof(ATEMYouTubeKeyer))
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,

                // ASLS key
                // ApiKey = "AI....",
                ApplicationName = nameof(ATEMYouTubeKeyer)
            });

            var broadcastsReq = youtubeService.LiveBroadcasts.List("id,snippet,contentDetails");
            broadcastsReq.BroadcastStatus = LiveBroadcastsResource.ListRequest.BroadcastStatusEnum.Upcoming;
            var broadcastsResponse = await broadcastsReq.ExecuteAsync();

            var broadcasts = new List<LiveBroadcastInfo>();
            foreach (var bi in broadcastsResponse.Items)
            {
                var streamsReq = youtubeService.LiveStreams.List("id,snippet,cdn");
                streamsReq.Id = bi.ContentDetails.BoundStreamId;

                var streamsResponse = await streamsReq.ExecuteAsync();
                foreach (var sr in streamsResponse.Items)
                {                    
                    var broadcastInfo = new LiveBroadcastInfo(bi.Snippet.Title,
                        bi.Snippet.ScheduledStartTime, bi.Snippet.ScheduledEndTime,
                        sr.Cdn.IngestionInfo);
                    broadcasts.Add(broadcastInfo);
                }
            }

            broadcasts.Sort(CompareBroadcasts);

            return broadcasts;
        }

        private static int CompareBroadcasts(LiveBroadcastInfo x, LiveBroadcastInfo y)
        {
            if (x.Start.HasValue && y.Start.HasValue)
            {
                return x.Start.Value.CompareTo(y.Start.Value);
            }
            return 0;
        }
    }
}
