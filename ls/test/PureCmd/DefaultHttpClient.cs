using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PureCmd
{
    public class HttpClientMgr
    {
        static List<HttpClient> _client = new List<HttpClient>();

        public static HttpClient CreateNew()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0;Windows NT 6.2; WOW64; Trident/6.0)");
            _client.Add(client);
            return client;
        }
    }
}
