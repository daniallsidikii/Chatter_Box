
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp
{
    public partial class MainWindow : Window
    {
        private TcpClient? client;  // Mark as nullable
        private NetworkStream? stream;
        private StreamReader? reader;
        private StreamWriter? writer;
        private Thread? listenThread;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };

                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }

        private void ListenForMessages()
        {
            try
            {
                while (true)
                {
                    if (reader == null) return;
                    string? message = reader.ReadLine();
                    if (message == null) break;

                    Dispatcher.Invoke(() => ChatMessages.Items.Add("Friend: " + message));
                }
            }
            catch (Exception) { }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrEmpty(message) && writer != null)
            {
                writer.WriteLine(message);
                ChatMessages.Items.Add("Me: " + message);
                MessageInput.Clear();
            }
        }
    }
}
