using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Server");

int port = 5000;
Socket listener = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp
);

IPAddress address = IPAddress.Any;
IPEndPoint endpoint = new IPEndPoint(address, port);

listener.Bind(endpoint);
listener.Listen(10);
Console.WriteLine("Server is listening on " + endpoint);

DateTime startedAt = DateTime.Now;

while (true)
{
    Socket client = listener.Accept();
    _ = Task.Run(() =>
    {
        Console.WriteLine("Client connected: " + client.RemoteEndPoint);

        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = client.Receive(buffer);
            if (bytesRead == 0)
            {
                break;
            }

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            string[] parts = request.Split(' ');
            string response;

            if (request == "1")
            {
                response = DateTime.Now.ToString("HH:mm:ss");
            }
            else if (request == "2")
            {
                response = DateTime.Now.ToString("dd.MM.yyyy");
            }
            else if (request == "3")
            {
                Random random = new Random();
                response = random.Next(1, 101).ToString();
            }
            else if (request == "4")
            {
                TimeSpan uptime = DateTime.Now - startedAt;
                response = uptime.ToString();
            }
            else if (request == "5")
            {
                response = "Goodbye!";
            }
            else if (parts[0] == "6")
            {
                int value = int.Parse(parts[1]);
                response = (value * value).ToString();
            }
            else if (parts[0] == "7")
            {
                int first = int.Parse(parts[1]);
                int second = int.Parse(parts[2]);
                response = (first + second).ToString();
            }
            else
            {
                response = "Unknown request";
            }

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            client.Send(responseBytes);
            Console.WriteLine("Response sent: " + response);

            if (request == "5")
            {
                break;
            }
        }

        client.Close();
        Console.WriteLine("Client disconnected");
    });
}
