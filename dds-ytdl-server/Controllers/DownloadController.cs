using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace dds_ytdl_server.Controllers
{
    [Produces("application/force-download")]    
    [Route("/[controller]")]
    public class DownloadController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var client = new YoutubeClient();
            var streamInfoSet = client.GetVideoMediaStreamInfosAsync(id).Result;
            var title = client.GetVideoAsync(id).Result.Title;
            var streamInfo = streamInfoSet.Audio.WithHighestBitrate();
            var inputStream = new MemoryStream();
            await client.DownloadMediaStreamAsync(streamInfo, inputStream);
            return File(inputStream.GetBuffer(), "application/force-download", $"{title}.mp3");
        }
    }
}