using System.Net;
using System;
using System.IO;
using System.Collections.Specialized;

namespace ExpressSharp
{
    public class Request
    {
        public HttpListenerRequest request;
        public NameValueCollection query;
        public string body;

        public Request(HttpListenerRequest httpRequest)
        {
            request = httpRequest;
            query = request.QueryString;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                body = reader.ReadToEnd();
            }
        }
    }
}