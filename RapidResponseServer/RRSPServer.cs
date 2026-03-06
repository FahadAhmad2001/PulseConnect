using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RapidResponseServer
{
    public class RRSPServer
    {
        HttpListener listener;
        public async void StartServer(int port)
        {
            listener = new HttpListener();
            //X509Certificate2 certificate = new X509Certificate2("testcert\");
            listener.Prefixes.Add("http://+:10500/login/");
            listener.Prefixes.Add("http://+:10500/createuser/");
            listener.Start();
            while (listener.IsListening)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                try
                {
                    await ProcessRequest(context);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async Task ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            Console.WriteLine("received a request" + request.RawUrl);
            Console.WriteLine("received a request" + request.Url);
            if (request.RawUrl.Contains(":10500/createuser?username="))
            {
                // example is /createuser?username=admin&password=password&rank=
                //string 
            }
        }
    }
}
