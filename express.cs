﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpressSharp
{
    public class Request
    {
        public HttpListenerRequest request;

        public Request(HttpListenerRequest req)
        {
            request = req;
        }
    }

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

    public class express
    {

        public string host = "localhost";
        public string protocol = "http";

        HttpListener listener;
        Dictionary<Tuple<string, HttpMethod>, Action<Request, Response>> routes;
        Dictionary<string, Action<Request, Response>> middlewares;
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

            var req = new Request(context.Request);
            var res = new Response(context.Response);
            var path = context.Request.Url.AbsolutePath;

            var method = context.Request.HttpMethod;

            var tuple = Tuple.Create(path, new HttpMethod(method));

            if (middlewares.ContainsKey(path))
            {
                middlewares[path](req, res);
            }
            if (routes.ContainsKey(tuple))
            {
                routes[tuple](req, res);
            }
            else
            {
                res.send($"Cannot {method} " + path);
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