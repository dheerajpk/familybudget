using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialDB.Http
{
    internal class HttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService()
        {
            _client = new HttpClient();
        }

        public Task<string> GetAysnc(string requestUri)
        {
            return _client.GetStringAsync(requestUri);
        }


    }
}
