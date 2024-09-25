using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace ChatGTP
{
    internal class Server
    {
        IPAddress localAddr;
        TcpListener server;
        Hashtable clients, privMessaging;
        bool running = false;
        List<TcpClient> admins;
        List<TcpClient> muted;
        public event EventHandler<(TcpClient, string)> ClientConnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<Exception> ExceptionReceived;

        private string[] adminNameList = { "admin" };
        public Server(string hostname="127.0.0.1", int port = 5000)
        {
            admins = new List<TcpClient>();
            muted = new List<TcpClient>();
            this.localAddr = IPAddress.Parse(hostname);
            this.server = new TcpListener(localAddr, port);
            this.clients = new Hashtable();
            this.privMessaging = new Hashtable();
        }

        public void Start()
        {
            running = true;
            server.Start();
            server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
        }

        public void Stop()
        {
            running = false;
            server.Stop();
            foreach (DictionaryEntry client in clients)
            {
                CloseClient((TcpClient)client.Key);
            }
        }

        public void OnClientConnect(IAsyncResult ar)
        {
            if (!running)
                return;
            TcpClient client = server.EndAcceptTcpClient(ar);
            try
            {
                server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnClientRead), (client, buffer));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
                CloseClient(client);
            }
        }

        public void OnClientRead(IAsyncResult ar)
        {
            (TcpClient client, byte[] buffer) = ((TcpClient, byte[]))ar.AsyncState;
            try
            {
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.EndRead(ar);
                if (bytesRead == 0)
                {
                    string nickname = clients[client].ToString();
                    CloseClient(client);
                    Broadcast(FormatMessage("has left the chat", nickname, admins.Contains(client), " "));
                    return;
                }
                if (muted.Contains(client))
                {
                    Send("You cannot speak here", client);
                    stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnClientRead), (client, buffer));
                    return;
                }
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                int nickLength = int.Parse(data.Split(',')[0]);
                string nick = data.Substring(nickLength.ToString().Length + 1, nickLength);
                string commandData = data.Substring(nickLength.ToString().Length + 1 + nickLength + 1);
                int command = int.Parse(commandData.Split(',')[0]);
                int commandArgLength = int.Parse(commandData.Split(',')[1]);
                string commandArg = commandData.Substring(commandData.Length - commandArgLength);
                if (!clients.ContainsKey(client))
                {
                    if (!CheckNickname(nick))
                    {
                        CloseClient(client);
                        return;
                    }
                    clients.Add(client, nick);
                    ClientConnected?.Invoke(this, (client, nick));
                    if (adminNameList.Contains(nick))
                    {
                        admins.Add(client);
                        Send("ADMIN", client);
                    }
                    Broadcast("MEMBERS: " + string.Join(",", GetMemberNicks()));
                    Broadcast(FormatMessage("has joined the chat", nick, admins.Contains(client), " "));
                }
                else if (clients[client].ToString() != nick)
                {
                    if (!CheckNickname(nick))
                    {
                        CloseClient(client);
                        return;
                    }
                    string oldNick = clients[client].ToString();
                    clients[client] = nick;
                    Broadcast(FormatMessage("has changed their nickname to " + nick, oldNick, admins.Contains(client), " "));
                }
                if (command == 1)
                {
                    if (commandArg == "quit")
                    {
                        Broadcast(FormatMessage("has left the chat", nick, admins.Contains(client), " "));
                        CloseClient(client);
                        return;
                    }
                    if (commandArg == "view-managers")
                    {
                        string[] adminNicks = new string[admins.Count];
                        for (int i = 0; i < admins.Count; i++)
                        {
                            adminNicks[i] = clients[admins[i]].ToString();
                        }
                        Send("Managers: " + string.Join(", ", adminNicks), client);
                    }
                    else if (privMessaging.Contains(client))
                    {
                        string msg = FormatMessage(commandArg, nick, admins.Contains(client), ": ", true);
                        Send(msg, (TcpClient)privMessaging[client]);
                        Send(msg, client);
                        MessageReceived?.Invoke(this, msg);
                        privMessaging.Remove(client);
                    }
                    else
                        Broadcast(FormatMessage(commandArg, nick, admins.Contains(client)));
                }
                else if (command == 2)
                {
                    if (admins.Contains(client))
                    {
                        foreach (DictionaryEntry clientEntry in clients)
                        {
                            if (clientEntry.Value.ToString() == commandArg)
                            {
                                TcpClient promotedClient = (TcpClient)clientEntry.Key;
                                if (admins.Contains(promotedClient))
                                {
                                    Send("This user is already a manager", client);
                                    break;
                                }
                                if (muted.Contains(promotedClient))
                                {
                                    Send("This user is muted", client);
                                    break;
                                }
                                if (promotedClient == client)
                                {
                                    Send("You cannot promote yourself", client);
                                    break;
                                }
                                admins.Add(promotedClient);
                                Broadcast(FormatMessage("has been promoted to manager", commandArg, true, " "));
                                Send("ADMIN", promotedClient);
                                break;
                            }
                        }
                    }
                }
                else if (command == 3)
                {
                    if (admins.Contains(client))
                    {
                        foreach (DictionaryEntry clientEntry in clients)
                        {
                            if (clientEntry.Value.ToString() == commandArg)
                            {
                                TcpClient kickedClient = (TcpClient)clientEntry.Key;
                                if (admins.Contains(kickedClient))
                                {
                                    Send("This user is a manager", client);
                                    break;
                                }
                                if (kickedClient == client)
                                {
                                    Send("You cannot kick yourself", client);
                                    break;
                                }
                                Broadcast(FormatMessage("has been kicked", commandArg, false, " "));
                                CloseClient(kickedClient);
                                break;
                            }
                        }
                    }
                }
                else if (command == 4)
                {
                    if (admins.Contains(client))
                    {
                        foreach (DictionaryEntry clientEntry in clients)
                        {
                            if (clientEntry.Value.ToString() == commandArg)
                            {
                                TcpClient mutedClient = (TcpClient)clientEntry.Key;
                                if (muted.Contains(mutedClient))
                                {
                                    Send("This user is already muted", client);
                                    break;
                                }
                                if (admins.Contains(mutedClient))
                                {
                                    Send("This user is a manager", client);
                                    break;
                                }
                                if (mutedClient == client)
                                {
                                    Send("You cannot mute yourself", client);
                                    break;
                                }
                                muted.Add(mutedClient);
                                Broadcast(FormatMessage("has been muted", commandArg, false, " "));
                                break;
                            }
                        }
                    }
                }
                else if (command == 5)
                {
                    foreach (DictionaryEntry clientEntry in clients)
                    {
                        if (clientEntry.Value.ToString() == commandArg)
                        {
                            TcpClient privateClient = (TcpClient)clientEntry.Key;
                            if (privateClient == client)
                            {
                                Send("You cannot message yourself", client);
                                break;
                            }
                            if (privMessaging.Contains(client))
                                privMessaging[client] = privateClient;
                            else
                                privMessaging.Add(client, privateClient);
                            break;
                        }
                    }
                }
                
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnClientRead), (client, buffer));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
                if (client == null || !clients.ContainsKey(client))
                {
                    return;
                }
                string nick = clients[client].ToString();
                CloseClient(client);
                string msg = FormatMessage("has left the chat", nick, admins.Contains(client), " ");
                Broadcast(msg);
            }
        }

        public void CloseClient(TcpClient client)
        {
            try
            {
                if (admins.Contains(client))
                    admins.Remove(client);
                if (muted.Contains(client))
                    muted.Remove(client);
                if (privMessaging.Contains(client))
                    privMessaging.Remove(client);
                if (clients.Contains(client))
                    clients.Remove(client);
                client.GetStream().Close();
                client.Close();
                Broadcast("MEMBERS: " + string.Join(",", GetMemberNicks()));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }

        public void Send(string message, TcpClient client)
        {
            string msg = message.Length + "," + message;
            client.GetStream().BeginWrite(Encoding.ASCII.GetBytes(msg), 0, msg.Length, null, null);
        }

        public void Broadcast(string message)
        {
            MessageReceived?.Invoke(this, message);
            foreach (DictionaryEntry client in clients)
            {
                Send(message, (TcpClient)client.Key);
            }
            Thread.Sleep(100);
        }

        private string FormatMessage(string message, string nick, bool admin = false, string seperator=": ", bool privMessage=false)
        {
            return DateTime.Now.ToString("HH:mm") + " " + (privMessage ? "!" : "") + (admin ? "@" : "") + nick + seperator + message;
        }

        private bool CheckNickname(string nickname)
        {
            if (nickname[0] == '@' || nickname[0] == '!')
                return false;
            foreach (char c in nickname)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }
            foreach (DictionaryEntry client in clients)
            {
                if ((string)client.Value == nickname)
                    return false;
            }
            return true;
        }

        private string[] GetMemberNicks()
        {
            string[] nicks = new string[clients.Count];
            int i = 0;
            foreach (DictionaryEntry client in clients)
            {
                nicks[i] = (admins.Contains(client.Key) ? "@" : "") + (string)client.Value;
                i++;
            }
            return nicks;
        }
    }
}
