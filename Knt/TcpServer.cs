using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Knt
{
    internal class TcpServer
    {
        public string name { get; set; }
        private int port { get; set; }
        private Socket socket { get; set; }
        List<Socket> users = new List<Socket>();

        public event Action<string> MessageReceived;
        private CancellationTokenSource cancellationTokenSource;
        public TcpServer(string Name, int Port)
        {
            name = Name;
            port = Port;
        }
        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(10);
            ListenToUsers(socket);

        }
        private async void ListenToUsers(Socket c)
        {
            while (true)
            {
                var client = await c.AcceptAsync();
                users.Add(client);
                ListenToMessage(client, users);
            }
        }
        private async void ListenToMessage(Socket c, List<Socket> users)
        {
            try
            {
                while (true)
                {
                    byte[] by = new byte[1024];
                    int bytesRead = await c.ReceiveAsync(by, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    string message = Encoding.UTF8.GetString(by, 0, bytesRead);
                    MessageReceived.Invoke(message);
                    SendMessageToAllClients(message);
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                while (true)
                {
                    byte[] by = new byte[1024];
                    int bytesRead = await c.ReceiveAsync(by, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    string message = Encoding.UTF8.GetString(by, 0, bytesRead);
                    MessageReceived.Invoke(message);
                    SendMessageToAllClients(message);
                }
            }
            catch (ObjectDisposedException)
            {
                while (true)
                {
                    byte[] by = new byte[1024];
                    int bytesRead = await c.ReceiveAsync(by, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    string message = Encoding.UTF8.GetString(by, 0, bytesRead);
                    MessageReceived.Invoke(message);
                    SendMessageToAllClients(message);
                }
            }
            catch (Exception ex)
            {
                while (true)
                {
                    byte[] by = new byte[1024];
                    int bytesRead = await c.ReceiveAsync(by, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    string message = Encoding.UTF8.GetString(by, 0, bytesRead);
                    MessageReceived.Invoke(message);
                    SendMessageToAllClients(message);
                }
            }
        }
        public async void SwndMessage(Socket client, string m)
        {
            var bytes = Encoding.UTF8.GetBytes(m);
            await client.SendAsync(bytes, SocketFlags.None);
        }
        public void SendMessageToAllClients(string message)
        {
            foreach (var userSocket in users)
            {
                SwndMessage(userSocket, message);
            }
        }
        public void Disconnect()
        {
            socket.Close();
            cancellationTokenSource.Cancel();
        }
    }
}
