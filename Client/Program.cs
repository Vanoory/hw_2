using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Client");

int port = 5000;
IPAddress address = IPAddress.Loopback;
IPEndPoint endpoint = new IPEndPoint(address, port);

Socket client = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp
);

client.Connect(endpoint);
Console.WriteLine("Connected to server: " + client.RemoteEndPoint);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== MENU ===");
    Console.WriteLine("1 - Get current time");
    Console.WriteLine("2 - Get current date");
    Console.WriteLine("3 - Get random number");
    Console.WriteLine("4 - Get server uptime");
    Console.WriteLine("5 - Exit");
    Console.WriteLine("6 - Square a number");
    Console.WriteLine("7 - Sum two numbers");
    Console.Write("Choose: ");

    string choice = Console.ReadLine() ?? "";
    string request = choice;

    if (choice == "6")
    {
        Console.Write("Enter a number: ");
        string number = Console.ReadLine() ?? "";
        request = choice + " " + number;
    }
    else if (choice == "7")
    {
        Console.Write("Enter the first number: ");
        string first = Console.ReadLine() ?? "";
        Console.Write("Enter the second number: ");
        string second = Console.ReadLine() ?? "";
        request = choice + " " + first + " " + second;
    }

    byte[] requestBytes = Encoding.UTF8.GetBytes(request);
    client.Send(requestBytes);

    byte[] responseBuffer = new byte[1024];
    int responseLength = client.Receive(responseBuffer);
    string response = Encoding.UTF8.GetString(responseBuffer, 0, responseLength);
    Console.WriteLine("Server response: " + response);

    if (choice == "5")
    {
        break;
    }
}

client.Close();
Console.WriteLine("Disconnected from server");
