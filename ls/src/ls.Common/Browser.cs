using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ls.Common
{
    internal class BrowserClient : WebClient
    {
        public int Timeout = 10000;
        public CookieContainer RequestCookie { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = RequestCookie;
                httpRequest.Timeout = Timeout;
            }
            return request;
        }

        #region GetResponseCookie

        public CookieCollection ResponseCookie { get; set; }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var resp = base.GetWebResponse(request);
            var http_resp = resp as HttpWebResponse;
            ResponseCookie = http_resp.Cookies;
            return resp;
        }
        #endregion
    }
    public class Browser
    {
        BrowserClient _client = new BrowserClient();
        public CookieCollection Cookie = new CookieCollection();
        /// <summary>
        /// 请求超时设置,单位:ms,默认10秒
        /// </summary>
        public int Timeout
        {
            get
            {
                return _client.Timeout;
            }
            set
            {
                _client.Timeout = value;
            }
        }

        #region ctor
        public Browser() : this(Encoding.UTF8)
        {
        }
        public Browser(Encoding encoding)
        {
            _client.Encoding = encoding;
        }
        #endregion

        public string Get(string url)
        {
            _client.RequestCookie = new CookieContainer();
            _client.RequestCookie.Add(Cookie);
            var document = _client.DownloadString(url);
            Cookie = _client.ResponseCookie;
            return document;
        }

        #region SetProxy
        public void SetProxy(string ip_port)
        {
            _client.Proxy = new WebProxy(ip_port);
        }
        public void SetProxy(string ip_port, string userName, string password)
        {
            SetProxy(ip_port);
            _client.Proxy.Credentials = new NetworkCredential(userName, password);
        }
        public void SetProxy(string ip, int port, string userName, string password)
        {
            SetProxy(ip + ":" + port, userName, password);
        }
        #endregion
    }
}