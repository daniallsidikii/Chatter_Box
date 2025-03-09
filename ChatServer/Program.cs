
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
