using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PulseConnectServerOld
{
    public class MainServer
    {
        HttpListener listener;
        public async void StartServer(int port)
        {
            listener = new HttpListener();
            //X509Certificate2 certificate = new X509Certificate2("testcert\");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/login/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/createuser/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/login/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/cookie-login/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/refresh-list/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/createstandaloneuser/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/login-standalone-user/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/cookie-login-standalone/");
            listener.Prefixes.Add("http://+:" + port.ToString() + "/refresh-standalone-list/");
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
            if (request.RawUrl.Contains(":10500/createstandaloneuser?username="))
            {
                // example is 127.0.0.1:10500/createstandaloneuser?username=newuser1&password=testpass&usertype=doctor&startevents=true&alertevents=true&grpname=unit-A&transfer=false
                Console.WriteLine("Received request to create standalone user");
                //if(request.QueryString.GetValues())
                // example is /createuser?username=admin&password=password&rank=
                //string 
            }
        }
    }
}
