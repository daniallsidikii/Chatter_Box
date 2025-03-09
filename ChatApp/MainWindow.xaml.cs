// using System;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;
// using System.Windows;
// using System.Windows.Threading;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client = null!;
//         private NetworkStream stream = null!;

//         public MainWindow()
//         {
//             InitializeComponent();
//             ConnectToServer();
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("127.0.0.1", 5000);
//                 stream = client.GetStream();
//                 ChatBox.AppendText("Connected to server...\n");  // ✅ Fixed .Show() error

//                 // Start listening for messages
//                 Thread receiveThread = new Thread(ReceiveMessages);
//                 receiveThread.IsBackground = true;
//                 receiveThread.Start();
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.AppendText("Connection failed: " + ex.Message);
//             }
//         }

//         private void SendMessage_Click(object sender, RoutedEventArgs e)
//         {
//             string message = MessageBox.Text.Trim();
//             if (!string.IsNullOrEmpty(message))
//             {
//                 byte[] data = Encoding.UTF8.GetBytes(message);
//                 stream.Write(data, 0, data.Length);
//                 ChatBox.AppendText("You: " + message + "\n");  // ✅ Fixed .Show() error
//                 MessageBox.Clear();
//             }
//         }

//         private void ReceiveMessages()
//         {
//             try
//             {
//                 byte[] buffer = new byte[1024];
//                 while (true)
//                 {
//                     int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                     if (bytesRead == 0) break;

//                     string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                     Dispatcher.Invoke(() => ChatBox.AppendText("Server: " + receivedMessage + "\n"));  // ✅ Fixed .Show() error
//                 }
//             }
//             catch (Exception)
//             {
//                 Dispatcher.Invoke(() => ChatBox.AppendText("Disconnected from server.\n"));
//             }
//         }
//     }
// }



// using System;
// using System.Collections.ObjectModel;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;
// using System.Windows;
// using System.Windows.Controls;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client;
//         private NetworkStream stream;
//         public ObservableCollection<string> Messages { get; set; } = new();

//         public MainWindow()
//         {
//             InitializeComponent();
//             client = new TcpClient();  // Initialize TCP client
//             stream = null!;            // Stream will be set when connected
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("127.0.0.1", 5000);
//                 stream = client.GetStream();

//                 Messages.Add("Connected to Server ✅");

//                 // Start receiving messages
//                 Thread receiveThread = new Thread(ReceiveMessages);
//                 receiveThread.IsBackground = true;
//                 receiveThread.Start();
//             }
//             catch (Exception ex)
//             {
//                 Messages.Add("Error: " + ex.Message);
//             }
//         }

//         private void SendMessage_Click(object sender, RoutedEventArgs e)
//         {
//             string message = MessageBox.Text.Trim();
//             if (!string.IsNullOrEmpty(message))
//             {
//                 byte[] data = Encoding.UTF8.GetBytes(message);
//                 stream.Write(data, 0, data.Length);

//                 // Show sender message on the right
//                 Messages.Add("🟢 You: " + message);
//                 MessageBox.Clear();
//             }
//         }

//         private void ReceiveMessages()
//         {
//             try
//             {
//                 byte[] buffer = new byte[1024];
//                 while (true)
//                 {
//                     int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                     if (bytesRead == 0) break;

//                     string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

//                     // Show received message on the left
//                     Dispatcher.Invoke(() => Messages.Add("🔵 Friend: " + receivedMessage));
//                 }
//             }
//             catch (Exception)
//             {
//                 Dispatcher.Invoke(() => Messages.Add("Disconnected ❌"));
//             }
//         }
//     }
// }



// using System;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading.Tasks;
// using System.Windows;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client;
//         private NetworkStream stream;

//         public MainWindow()
//         {
//             InitializeComponent();
//             ConnectToServer();
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("127.0.0.1", 5000);
//                 stream = client.GetStream();
//                 Task.Run(ReceiveMessages); // Start listening for incoming messages
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show("⚠️ Connection failed: " + ex.Message);
//             }
//         }

//         private async void ReceiveMessages()
//         {
//             byte[] buffer = new byte[1024];

//             try
//             {
//                 while (client.Connected)
//                 {
//                     int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
//                     if (bytesRead > 0)
//                     {
//                         string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
//                         Dispatcher.Invoke(() => chatTextBox.AppendText(receivedMessage + "\n"));
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Dispatcher.Invoke(() => MessageBox.Show("❌ Connection lost: " + ex.Message));
//             }
//         }

//         private void SendMessage(object sender, RoutedEventArgs e)
//         {
//             try
//             {
//                 if (client != null && stream != null)
//                 {
//                     string message = messageTextBox.Text.Trim();
//                     if (!string.IsNullOrEmpty(message))
//                     {
//                         byte[] data = Encoding.ASCII.GetBytes(message);
//                         stream.Write(data, 0, data.Length);

//                         chatTextBox.AppendText("Me: " + message + "\n");
//                         messageTextBox.Clear();
//                     }
//                 }
//                 else
//                 {
//                     MessageBox.Show("⚠️ Not connected to the server.");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show("❌ Error: " + ex.Message);
//             }
//         }
//     }
// }



// using System;
// using System.Windows;
// using System.Net.Sockets;
// using System.IO;
// using System.Threading;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client;
//         private StreamReader reader;
//         private StreamWriter writer;
//         private Thread listenThread;

//         public MainWindow()
//         {
//             InitializeComponent();
//             ConnectToServer();
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("SERVER_IP_HERE", 5000); // Change SERVER_IP_HERE to server's IP
//                 NetworkStream stream = client.GetStream();
//                 reader = new StreamReader(stream);
//                 writer = new StreamWriter(stream) { AutoFlush = true };

//                 listenThread = new Thread(ReceiveMessages);
//                 listenThread.Start();
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show("Error connecting to server: " + ex.Message);
//             }
//         }

//         private void ReceiveMessages()
//         {
//             try
//             {
//                 while (true)
//                 {
//                     string message = reader.ReadLine();
//                     if (message != null)
//                     {
//                         Dispatcher.Invoke(() => chatHistory.Text += "Friend: " + message + "\n");
//                     }
//                 }
//             }
//             catch
//             {
//                 MessageBox.Show("Disconnected from server.");
//             }
//         }

//         private void SendMessage(object sender, RoutedEventArgs e)
//         {
//             if (!string.IsNullOrWhiteSpace(messageInput.Text))
//             {
//                 writer.WriteLine(messageInput.Text);
//                 chatHistory.Text += "Me: " + messageInput.Text + "\n";
//                 messageInput.Clear();
//             }
//         }
//     }
// }



// using System;
// using System.IO;
// using System.Net.Sockets;
// using System.Threading;
// using System.Windows;
// using System.Windows.Controls;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client;
//         private StreamReader reader;
//         private StreamWriter writer;
//         private Thread listenThread;

//         public MainWindow()
//         {
//             InitializeComponent();
//             ConnectToServer();
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("127.0.0.1", 5000);
//                 NetworkStream stream = client.GetStream();
//                 reader = new StreamReader(stream);
//                 writer = new StreamWriter(stream) { AutoFlush = true };

//                 listenThread = new Thread(ListenForMessages);
//                 listenThread.Start();
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show("Error connecting to server: " + ex.Message);
//             }
//         }

//         private void ListenForMessages()
//         {
//             try
//             {
//                 while (true)
//                 {
//                     string message = reader.ReadLine();
//                     if (message == null) break;

//                     Dispatcher.Invoke(() => ChatMessages.Items.Add("Friend: " + message));
//                 }
//             }
//             catch (Exception) { }
//         }

//         private void SendMessage_Click(object sender, RoutedEventArgs e)
//         {
//             string message = MessageInput.Text;
//             if (!string.IsNullOrEmpty(message))
//             {
//                 writer.WriteLine(message);
//                 ChatMessages.Items.Add("Me: " + message);
//                 MessageInput.Clear();
//             }
//         }
//     }
// }



// using System;
// using System.IO;
// using System.Net.Sockets;
// using System.Threading;
// using System.Windows;
// using System.Windows.Controls;

// namespace ChatApp
// {
//     public partial class MainWindow : Window
//     {
//         private TcpClient client;
//         private NetworkStream stream;
//         private StreamReader reader;
//         private StreamWriter writer;
//         private Thread listenThread;

//         public MainWindow()
//         {
//             InitializeComponent();
//             ConnectToServer();
//         }

//         private void ConnectToServer()
//         {
//             try
//             {
//                 client = new TcpClient("127.0.0.1", 5000);
//                 stream = client.GetStream();
//                 reader = new StreamReader(stream);
//                 writer = new StreamWriter(stream) { AutoFlush = true };

//                 listenThread = new Thread(ListenForMessages);
//                 listenThread.IsBackground = true;
//                 listenThread.Start();
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show("Error connecting to server: " + ex.Message);
//             }
//         }

//         private void ListenForMessages()
//         {
//             try
//             {
//                 while (true)
//                 {
//                     string message = reader.ReadLine();
//                     if (message == null) break;

//                     Dispatcher.Invoke(() => ChatMessages.Items.Add("Friend: " + message));
//                 }
//             }
//             catch (Exception) { }
//         }

//         private void SendMessage_Click(object sender, RoutedEventArgs e)
//         {
//             string message = MessageInput.Text;
//             if (!string.IsNullOrEmpty(message))
//             {
//                 writer.WriteLine(message);
//                 ChatMessages.Items.Add("Me: " + message);
//                 MessageInput.Clear();
//             }
//         }
//     }
// }



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
