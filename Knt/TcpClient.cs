using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Knt
{
    internal class TcpClient
    {
        private string serverAddress { get; set; }
        private int port { get; set; }
        public string name { get; set; }
        private Socket clientSocket { get; set; }
        public event Action<string> MessageReceived;
        private CancellationTokenSource cancellationTokenSource ;

        public TcpClient(string ServerAddress, int Port, string Name)
        {
            serverAddress = ServerAddress;
            port = Port;
            name = Name;
        }

        public async Task ConnectAsync()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await clientSocket.ConnectAsync(serverAddress, port);
            cancellationTokenSource = new CancellationTokenSource();
            Listen(cancellationTokenSource.Token);
        }
        private async void Listen(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    byte[] by = new byte[1024];
                    int bytesRead = await clientSocket.ReceiveAsync(by, SocketFlags.None);
                    string message = Encoding.UTF8.GetString(by, 0, bytesRead);
                    MessageReceived.Invoke(message);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Что-то не так");
            }
        }
        public async Task SendMessageAsync(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await clientSocket.SendAsync(buffer, SocketFlags.None);
        }
        public void Disconnect()
        {
            clientSocket.Close();
            cancellationTokenSource.Cancel();
        }
    }
}
