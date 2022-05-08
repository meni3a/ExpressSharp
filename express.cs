using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpressSharp
{
    public class express
    {

        HttpListener listener;
        Dictionary<Tuple<string, HttpMethod>, Action<Request, Response>> routes;
        Dictionary<string, Action<Request, Response>> middlewares;
        public string host = "localhost";
        public string protocol = "http";
        
        public express()
        {
            listener = new HttpListener();
            routes = new Dictionary<Tuple<string, HttpMethod>, Action<Request, Response>>();
            middlewares = new Dictionary<string, Action<Request, Response>>();
        }

        public void stop()
        {
            listener.Stop();
        }

        public void use(string prefix, Action<Request, Response> callback)
        {
            middlewares[prefix] = callback;

        }

        public void listen(int port, Action callback)
        {
            string baseAddress = $"{protocol}://{host}:";
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener is not suppotred.");
                return;
            }

            if (listener.IsListening)
            {
                return;
            }

            foreach (var s in routes.Keys)
            {
                string url = baseAddress + port.ToString() + s.Item1 + "/";
                listener.Prefixes.Add(url);
            }

            listener.Start();
            callback();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {

                    var context = await listener.GetContextAsync();
                    handleIncomingRequest(context);

                };
            }, TaskCreationOptions.LongRunning);

        }


        private void handleIncomingRequest(HttpListenerContext context)
        {

            if (context.Request.Url is null)
            {
                throw new Exception("Fail to proccess request, context is null");
            }

            var request = new Request(context.Request);
            var response = new Response(context.Response);
            var path = context.Request.Url.AbsolutePath;

            var method = context.Request.HttpMethod;

            var tuple = Tuple.Create(path, new HttpMethod(method));

            if (middlewares.ContainsKey(path))
            {
                middlewares[path](request, response);
            }
            if (routes.ContainsKey(tuple))
            {
                routes[tuple](request, response);
            }
            else
            {
                response.send($"Cannot {method} " + path);
            }

        }

        public void get(string prefix, Action<Request, Response> callback)
        {
            routes[Tuple.Create(prefix, HttpMethod.Get)] = callback;
        }

        public void post(string prefix, Action<Request, Response> callback)
        {
            routes[Tuple.Create(prefix, HttpMethod.Post)] = callback;
        }

        public void put(string prefix, Action<Request, Response> callback)
        {
            routes[Tuple.Create(prefix, HttpMethod.Put)] = callback;
        }

        public void delete(string prefix, Action<Request, Response> callback)
        {
            routes[Tuple.Create(prefix, HttpMethod.Delete)] = callback;
        }

        public void patch(string prefix, Action<Request, Response> callback)
        {
            routes[Tuple.Create(prefix, HttpMethod.Patch)] = callback;
        }

    }

}