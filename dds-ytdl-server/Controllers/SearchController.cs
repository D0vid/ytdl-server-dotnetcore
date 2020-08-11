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
            var result = client.Search.GetVideosAsync(token,0,1).BufferAsync().Result;
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("video/{token}")]
        public string GetVideo(string token)
        {
            var client = new YoutubeClient();
            var result = client.Videos.GetAsync(token).Result;
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("playlist/{token}")]
        public string GetPlayList(string token)
        {
            var client = new YoutubeClient();
            var result = client.Playlists.GetVideosAsync(token).BufferAsync().Result;
            return JsonConvert.SerializeObject(result);
        }
    }
}