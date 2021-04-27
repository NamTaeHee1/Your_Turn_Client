using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Your_Turn_Client
{
    class TCP
     {
        TcpListener Server;
        TcpClient Client;

        const int PORT = 8000;
        byte[] SendBuffer = new byte[1024];
        byte[] SendBuffer1 = new byte[1024];
        NetworkStream Stream;

        public void StartServer()
        {
            Server = new TcpListener(IPAddress.Any, PORT);
            Server.Start();

            Client = Server.AcceptTcpClient();
            Stream = Client.GetStream();

            while (true)
            {
                string Message = "taehee";
                string Message1 = "6";
                SendBuffer = Encoding.ASCII.GetBytes(Message);
                Stream.Write(SendBuffer, 0, Message.Length);
                SendBuffer1 = Encoding.ASCII.GetBytes(Message1);
                Stream.Write(SendBuffer1, 0, Message1.Length);
            }
        }


        public void CloseServer()
        {
            if (Server != null) Server.Stop();
            if (Client != null) Client.Close();
        }
    }
}
