using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bend.Util
{
    public abstract class HttpServer
    {

        protected IPAddress server;
        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServer(int port)
        {
            this.server = new IPAddress(new byte[] { 127, 0, 0, 1 });
            this.port = port;
        }

        public HttpServer(IPAddress server, int port)
        {
            this.server = server;
            this.port = port;
        }

        public HttpServer(byte[] server, int port)
        {
            this.server = new IPAddress(server);
            this.port = port;
        }

        public void listen()
        {
            listener = new TcpListener(server, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

}
