using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test_OCRWebApi
{
    public class RestConnector
    {
        private string _uri;
        private static readonly HttpClient _httpClient;

        static RestConnector()
        {
            _httpClient = new HttpClient();
        }

        public RestConnector(string uri, double timeOutMS)
        {
            _uri = uri;
            _httpClient.BaseAddress = new Uri(uri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeOutMS));
        }

        public async Task<string> GetData(string api, string filePath)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            var b  = File.ReadAllBytes(filePath);
            multiContent.Add(new ByteArrayContent(b), "files", "files");
            var res = await _httpClient.PostAsync(api,multiContent).ConfigureAwait(false);
            return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
