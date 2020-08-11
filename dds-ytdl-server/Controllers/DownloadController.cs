using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

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
            var streamManifest = await client.Videos.Streams.GetManifestAsync(id);
            var title = client.Videos.GetAsync(id).Result.Title;
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            var inputStream = await client.Videos.Streams.GetAsync(streamInfo);
            var memStream = new MemoryStream();
            inputStream.CopyTo(memStream);
            return File(memStream.GetBuffer(), "application/force-download", $"{title}.mp3");
        }
    }
}