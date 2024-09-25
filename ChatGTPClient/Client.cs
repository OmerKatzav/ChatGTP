using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatGTPClient
{
    internal class Client
    {
        public event EventHandler<string> MessageReceived;
        public event EventHandler Disconnected;
        public event EventHandler<Exception> ExceptionReceived;
        public event EventHandler<string[]> MembersReceived;
        public event EventHandler AdminReceived;
        TcpClient client;
        String hostname;
        int port;
        string nickname;
        
        public Client(string nickname, string hostname="127.0.0.1", int port=5000)
        {
            this.nickname = nickname;
            this.hostname = hostname;
            this.port = port;
        }

        public void Start()
        {
            try
            {
                client = new TcpClient(hostname, port);
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }

        public void Stop()
        {
            client.Close();
        }

        public void OnRead(IAsyncResult ar)
        {
            try
            {
                (NetworkStream stream, byte[] buffer) = ((NetworkStream, byte[]))ar.AsyncState;
                int bytesRead = stream.EndRead(ar);
                if (bytesRead == 0)
                {
                    Stop();
                    Disconnected?.Invoke(this, EventArgs.Empty);
                    return;
                }
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                int messageLength = int.Parse(data.Split(',')[0]);
                string message = data.Substring(data.Length - messageLength, messageLength);
                if (message.StartsWith("MEMBERS: "))
                {
                    string[] members = message.Substring(9).Split(',');
                    MembersReceived?.Invoke(this, members);
                }
                else if (message == "ADMIN") AdminReceived?.Invoke(this, EventArgs.Empty);
                else MessageReceived?.Invoke(this, message);
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }

        public void Send(int command, string commandData)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(nickname.Length + "," + nickname + "," + command + "," + commandData.Length + "," + commandData);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }
    }
}
