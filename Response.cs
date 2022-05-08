using System.IO;
using System.Net;
using System.Text;


namespace ExpressSharp
{
    public class Response
    {
        public HttpListenerResponse response;

        public Response(HttpListenerResponse httpResponse)
        {
            response = httpResponse;
        }
        public Response status(int status)
        {
            response.StatusCode = status;
            return this;
        }
        public Response send(string data)
        {

            string responseString = data;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            return this;

        }

    }
}