using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YoutubeExplode;

namespace dds_ytdl_server.Controllers
{
    [Route("/[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpGet("{token}")]
        public string GetToken(string token)
        {
            var client = new YoutubeClient();
            var result = client.SearchVideosAsync(token, 1).Result;
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("video/{token}")]
        public string GetVideo(string token)
        {
            var client = new YoutubeClient();
            var result = client.GetVideoAsync(token).Result;
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("playlist/{token}")]
        public string GetPlayList(string token)
        {
            var client = new YoutubeClient();
            var result = client.GetPlaylistAsync(token).Result.Videos;
            return JsonConvert.SerializeObject(result);
        }
    }
}