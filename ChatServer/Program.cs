// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;

// class ChatServer
// {
//     static TcpListener server;
//     static void Main()
//     {
//         try
//         {
//             server = new TcpListener(IPAddress.Any, 5000);
//             server.Start();
//             Console.WriteLine("🚀 Server started... Waiting for clients...");

//             while (true)
//             {
//                 TcpClient client = server.AcceptTcpClient();
//                 Console.WriteLine("✅ Client connected!");
//                 Thread clientThread = new Thread(HandleClient);
//                 clientThread.Start(client);
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("Error: " + ex.Message);
//         }
//     }

//     static void HandleClient(object obj)
//     {
//         TcpClient client = (TcpClient)obj;
//         NetworkStream stream = client.GetStream();
//         byte[] buffer = new byte[1024];
//         int bytesRead;

//         try
//         {
//             while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
//             {
//                 string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
//                 Console.WriteLine("📩 Received: " + message);

//                 // Send the message back (echo server)
//                 byte[] response = Encoding.ASCII.GetBytes("Server: " + message);
//                 stream.Write(response, 0, response.Length);
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("❌ Client disconnected: " + ex.Message);
//         }
//         finally
//         {
//             client.Close();
//         }
//     }
// }


// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Net.Sockets;
// using System.IO;
// using System.Threading;

// class ChatServer
// {
//     static List<TcpClient> clients = new List<TcpClient>();

//     static void Main()
//     {
//         TcpListener server = new TcpListener(IPAddress.Any, 5000);
//         server.Start();
//         Console.WriteLine("🚀 Server started... Waiting for clients...");

//         while (true)
//         {
//             TcpClient client = server.AcceptTcpClient();
//             clients.Add(client);
//             Console.WriteLine("✅ Client connected!");

//             Thread clientThread = new Thread(HandleClient);
//             clientThread.Start(client);
//         }
//     }

//     static void HandleClient(object obj)
//     {
//         TcpClient client = (TcpClient)obj;
//         StreamReader reader = new StreamReader(client.GetStream());
//         StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

//         try
//         {
//             while (true)
//             {
//                 string message = reader.ReadLine();
//                 if (message == null) break;

//                 Console.WriteLine("📩 Received: " + message);
//                 BroadcastMessage(message, client);
//             }
//         }
//         catch
//         {
//             Console.WriteLine("⚠️ Client disconnected.");
//         }
//         finally
//         {
//             clients.Remove(client);
//             client.Close();
//         }
//     }

//     static void BroadcastMessage(string message, TcpClient sender)
//     {
//         foreach (TcpClient client in clients)
//         {
//             if (client != sender)
//             {
//                 StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
//                 writer.WriteLine(message);
//             }
//         }
//     }
// }



// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.IO;
// using System.Threading;

// class ChatServer
// {
//     private static TcpListener listener;
//     private static List<TcpClient> clients = new List<TcpClient>();

//     static void Main()
//     {
//         Console.WriteLine("Server started...");
//         listener = new TcpListener(IPAddress.Any, 5000);
//         listener.Start();

//         while (true)
//         {
//             TcpClient client = listener.AcceptTcpClient();
//             clients.Add(client);
//             Console.WriteLine("Client connected!");

//             Thread clientThread = new Thread(HandleClient);
//             clientThread.Start(client);
//         }
//     }

//     private static void HandleClient(object obj)
//     {
//         TcpClient client = (TcpClient)obj;
//         NetworkStream stream = client.GetStream();
//         StreamReader reader = new StreamReader(stream);
//         StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

//         try
//         {
//             while (true)
//             {
//                 string message = reader.ReadLine();
//                 if (message == null) break;

//                 Console.WriteLine("Received: " + message);
//                 BroadcastMessage(message, client);
//             }
//         }
//         catch (Exception) { }
//         finally
//         {
//             clients.Remove(client);
//             client.Close();
//         }
//     }

//     private static void BroadcastMessage(string message, TcpClient sender)
//     {
//         foreach (var client in clients)
//         {
//             if (client != sender)
//             {
//                 StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
//                 writer.WriteLine(message);
//             }
//         }
//     }
// }



using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class ChatServer
{
    private static TcpListener listener;
    private static List<TcpClient> clients = new List<TcpClient>();

    static void Main()
    {
        Console.WriteLine("Server started...");
        listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("Client connected!");

            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

        try
        {
            while (true)
            {
                string message = reader.ReadLine();
                if (message == null) break;

                Console.WriteLine("Received: " + message);
                BroadcastMessage(message, client);
            }
        }
        catch (Exception) { }
        finally
        {
            clients.Remove(client);
            client.Close();
        }
    }

    private static void BroadcastMessage(string message, TcpClient sender)
    {
        foreach (var client in clients)
        {
            if (client != sender)
            {
                StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                writer.WriteLine(message);
            }
        }
    }
}
