using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите ваше имя: ");
        string name = Console.ReadLine();
        Client client = new Client(name);
        client.Recieve();
        client.SendMess(name);
        client.Recieve();
        while (true)
        {
            Console.Write("Введите ваш вопрос: ");
            string que = Console.ReadLine();
            client.SendMess(que);
            client.Recieve();
        }


    }
}

public class Client
{
    private IPEndPoint endPoint;
    private Socket socket;
    public string Name { get; set; }
    public Client(string name)
    {
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7632);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Name = name;
        Connect();
    }

    public void Connect()
    {
        socket.Connect(endPoint);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Client connect");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void SendMess(string mess)
    {
        string sendmess = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}] Client: {mess}";
        byte[] buffer = Encoding.Unicode.GetBytes(sendmess);
        socket.Send(buffer);
    }
    public string Recieve()
    {
        var buff = new byte[1024];
        socket.Receive(buff);
        string mess = Encoding.Unicode.GetString(buff);
        Console.WriteLine(mess);
        return mess;
    }
}