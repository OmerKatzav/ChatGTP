using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatGTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
            Console.WriteLine("Server started");
            server.MessageReceived += (s, msg) =>
            {
                Console.WriteLine(msg);
            };
            server.ClientConnected += (s, clientInfo) =>
            {
                (TcpClient client, string nick) = clientInfo;
                IPEndPoint clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;

                Console.WriteLine(clientEndPoint.Address + ":" + clientEndPoint.Port + " " + nick);
            };
            server.ExceptionReceived += (s, err) =>
            {
                Console.WriteLine(err.Message);
            };
            while (true)
            {
                string message = Console.ReadLine();
                if (message == "!exit")
                {
                    server.Stop();
                    break;
                }
                server.Broadcast(message);
            }
        }
    }
}
