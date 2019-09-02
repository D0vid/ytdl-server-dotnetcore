using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
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

            // Convert stream
            var titleHash = GetSha384HashString(title);

            var tempInputFilePath = $"Temp/{titleHash}_toconvert.webm";
            var tempOutputFilePath = $"Temp/{titleHash}_converted.mp3";

            using (var fs = new FileStream(tempInputFilePath, FileMode.Create))
            {
                fs.Write(inputStream.ToArray(),0,inputStream.ToArray().Length);
            }

            await Conversion.Convert(tempInputFilePath, tempOutputFilePath).Start();

            var bytes = System.IO.File.ReadAllBytes(tempOutputFilePath);
            System.IO.File.Delete(tempInputFilePath);
            System.IO.File.Delete(tempOutputFilePath);
            return File(bytes, "application/force-download", $"{title}.mp3");
        }

        public static string GetSha384HashString(string toHash)
        {
            return RemoveDashes(BitConverter.ToString(new SHA384Managed().ComputeHash(Encoding.UTF8.GetBytes(toHash))));
        }

        private static string RemoveDashes(string dashedHexString)
        {
            return dashedHexString.Replace("-", string.Empty).ToLower();
        }
    }
}