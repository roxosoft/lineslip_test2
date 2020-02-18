using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Runtime.Caching;
using System.Text;
using Tesseract;

namespace OCRWebApi.Controllers
{
    public class ValuesController : ApiController
    {
         
        public string Post()
        {
            var dirTessData  = HttpContext.Current.Server.MapPath(@"\")+ @"..\tessdata";

            var httpRequest = HttpContext.Current.Request;

            var inpStream = httpRequest.Files[0].InputStream;
            var hashCode = GetHash(inpStream);
            var res = MemoryCache.Default.Get(hashCode);

            //if cached get cached value
            if (res != null)
                return res as string;

            byte[] buff = new byte[inpStream.Length];
            inpStream.Read(buff, 0, (int)inpStream.Length);
            
            TesseractEngine engine =
                new TesseractEngine(
                    dirTessData, "eng",
                    EngineMode.Default);

            System.Drawing.Bitmap b = new System.Drawing.Bitmap(inpStream);
            Pix img = PixConverter.ToPix(b);
            var page = engine.Process(img);
            var text = page.GetText();
            
            MemoryCache.Default.Add(hashCode,text, DateTime.Now.AddMinutes(20));
            
            return text;
        }

        private string GetHash(Stream stream)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var h = md5.ComputeHash(stream);
           
            var sb = new StringBuilder();

            foreach (var e in h)
                sb.AppendFormat("{0:x2}", e);

            return sb.ToString();
        }


    }
}
