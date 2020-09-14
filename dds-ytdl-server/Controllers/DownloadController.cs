using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
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
            var converter = new YoutubeConverter(client);
            var streamManifest = await client.Videos.Streams.GetManifestAsync(id);
            var title = client.Videos.GetAsync(id).Result.Title;
            string path = $"{id}.mp3";
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            var mediaStreamInfos = new IStreamInfo[] { streamInfo };
            await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, path, "mp3");
            byte[] buff = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path); 
            return File(buff, "application/force-download", $"{title}.mp3"); 
        }
    }
}