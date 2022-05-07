using System.IO;
using System.Net;
using System.Text;


namespace ExpressSharp
{
    public class Response
    {
        public HttpListenerResponse response;

        public Response(HttpListenerResponse res)
        {
            response = res;
        }
        public Response status(int number)
        {
            response.StatusCode = number;
            return this;
        }
        public Response send(string str)
        {

            string responseString = str;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            return this;

        }

    }
}