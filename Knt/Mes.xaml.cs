using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Knt
{
    public partial class Mes : Window
    {
        private TcpServer server;
        private TcpClient client;
        public Mes(string text, int ii, string servaddress)
        {
            InitializeComponent();  
            switch (ii)
            {
                case 1:
                    username.Text = "Админ";
                    server = new TcpServer(text, 4040);
                    server.Start();
                    server.MessageReceived += TcpServer_MessageReceived;
                    break; 
                case 2:
                    username.Text = text;
                    client = new TcpClient(servaddress, 4040, text);
                    client.ConnectAsync();
                    client.MessageReceived += TcpClient_MessageReceived;
                    break;
            }

        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void TcpServer_MessageReceived(string message)
        {
            Dispatcher.Invoke(() =>
            {
                mess.Items.Add(message);
            });
        }

        private void TcpClient_MessageReceived(string message)
        {
            Dispatcher.Invoke(() =>
            {
                mess.Items.Add(message);
            });
        }

        private async void input_Click(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Now;
            if (message.Text != "/disconnect")
            {
                if(username.Text == "Админ")
                {
                    string messageText = $"Сообщение от [{time}] {server.name}: {message.Text}";
                    server.SendMessageToAllClients(messageText);
                    mess.Items.Add(messageText);
                    message.Text = "";
                }
                else
                {
                    string messageText = $"Сообщение от [{time}] {client.name}: {message.Text}";
                    await client.SendMessageAsync(messageText);
                    message.Text = "";
                }
            }
            else
            {
                this.Close();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (username.Text == "Админ")
            {
                server.Disconnect();
            }
            else
            {
                client.Disconnect();
            }
            base.OnClosing(e);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
