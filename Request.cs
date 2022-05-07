using System.Net;
using System;
using System.IO;

namespace ExpressSharp
{
    public class Request
    {
        public object query;
        public object body;
        public HttpListenerRequest request;

        public Request(HttpListenerRequest req)
        {
            request = req;
            query = request.QueryString;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                body = reader.ReadToEnd();
            }
        }
    }
}