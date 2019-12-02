using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreviewController : ControllerBase
    {

        [HttpGet]
        public string[] Get()
        {
            return new string[]{
                                  "https://www.google.com/cse/static/images/1x/googlelogo_grey_46x15dp.png",
                                  "aaaaaaaaa",
                                  "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Rotating_earth_%28large%29.gif/267px-Rotating_earth_%28large%29.gif",
                                  "https://images.ru.prom.st/433652141_w640_h640_dsc_1206.jpg"
                                };
        }

        
        [HttpPost]
        public PreviewPostRes[] Post([FromBody]string[] Url)
        {
            Size size = new Size(100, 100);
            int imgCount = Url.Length;
            var respons = new PreviewPostRes[imgCount];

            for (int i=0; i < imgCount; i++)
            {
                try
                {
                    Byte[] byteArr = ByteArrByURL(Url[i], size);
                    respons[i] = new PreviewPostRes(Convert.ToBase64String(byteArr), Url[i]);
                }
                catch
                {
                    respons[i] = new PreviewPostRes(null, Url[i]);
                }
            }
            return respons;
        }

        private static byte[] ByteArrByURL(string url, Size size)
        {
            ImageProcessor.Imaging.Formats.ISupportedImageFormat format = new ImageProcessor.Imaging.Formats.PngFormat();
            byte[] byteArr;

            var request = System.Net.WebRequest.Create(url);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(responseStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Save(outStream);
                    }
                byteArr = outStream.ToArray();
            }
            return byteArr;
        }
    }
}