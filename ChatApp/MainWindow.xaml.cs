using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatApp
{
    public partial class MainWindow : Window
    {
        private TcpClient? client;  // Mark as nullable
        private NetworkStream? stream;
        private StreamReader? reader;
        private StreamWriter? writer;
        private Thread? listenThread;
        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<Contact> Contacts { get; set; }
        public Contact? SelectedContact { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();
            Contacts = new ObservableCollection<Contact>();
            ChatMessages.ItemsSource = Messages;
            ContactsList.ItemsSource = Contacts;
            ConnectToServer();
            InitializePlaceholder();
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

                    Dispatcher.Invoke(() => Messages.Add(new Message
                    {
                        Sender = "Friend",
                        Content = message,
                        Timestamp = DateTime.Now // Correctly assign DateTime value
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show("Error receiving message: " + ex.Message));
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (!string.IsNullOrEmpty(message) && writer != null)
            {
                writer.WriteLine(message);
                Messages.Add(new Message
                {
                    Sender = "Me",
                    Content = message,
                    Timestamp = DateTime.Now // Correctly assign DateTime value
                });
                MessageInput.Clear();
                RestorePlaceholder();
            }
        }

        private void InitializePlaceholder()
        {
            MessageInput.Text = "Type a message...";
            MessageInput.Foreground = new SolidColorBrush(Colors.Gray);
            MessageInput.GotFocus += MessageInput_GotFocus;
            MessageInput.LostFocus += MessageInput_LostFocus;
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Type a message...")
            {
                MessageInput.Text = "";
                MessageInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                RestorePlaceholder();
            }
        }

        private void RestorePlaceholder()
        {
            MessageInput.Text = "Type a message...";
            MessageInput.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void ContactsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedContact = ContactsList.SelectedItem as Contact;
            if (SelectedContact != null)
            {
                // Update the chat messages for the selected contact
                // This is just a placeholder, you should load the actual messages for the selected contact
                Messages.Clear();
                Messages.Add(new Message
                {
                    Sender = SelectedContact.Name,
                    Content = "Hello!",
                    Timestamp = DateTime.Now // Correctly assign DateTime value
                });
            }
        }
    }

    public class Contact
    {
        public string Name { get; set; } = string.Empty; // Default to an empty string for safety
    }
}