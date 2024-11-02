using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        Game game = new Game();
        Server server = new Server();
        server.SendMess("START GAME");

        string name = server.Recieve();
        Data data = new Data(name);
        data.Dialog = data.ReadData();
        server.SendMess(data.Dialog);
        while (true)
        {
            string que = server.Recieve();
            data.AddBitDialog(que);
            string anw = game.GetWord(que);
            string send = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}] MagicBall: {anw}";
            data.AddBitDialog(send);
            server.SendMess(send);

            data.SaveData();
        }
    }
}

public class Server
{
    private IPAddress Ip;
    private int port;
    private Socket server;
    private Socket client;

    public Server()
    {
        Ip = IPAddress.Parse("127.0.0.1");
        port = 7632;
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Configuration();
        Listen();
    }
    public void Configuration()
    {
        IPEndPoint endPoint = new IPEndPoint(Ip, port);
        server.Bind(endPoint);
    }
    public void Listen()
    {
        server.Listen(1);
        client = server.Accept();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Client connect");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void SendMess(string mess)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(mess);
        client.Send(buffer);
    }

    public string Recieve()
    {
        var buff = new byte[1024];
        client.Receive(buff);
        string mess = Encoding.Unicode.GetString(buff);
        Console.WriteLine(mess);
        return mess;
    }



}
public class Game
{
    private string[] Words; //ДА нет не знаю возможно может быть
    Random random;
    public Game()
    {
        Words = new string[] { "Да", "Нет", "Не знаю", "Возможно", "Может быть" };
        random = new Random();
    }

    public string GetWord(string que)
    {
        return Words[random.Next(0, Words.Length)];
    }
}
public class Data
{
    public string NameClient { get; set; }
    public string Dialog { get; set; }
    public Data(string name)
    {
        NameClient = name;
        Dialog = "Start";
    }

    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter("data.txt", false))
        {
            writer.Write(Dialog);
        }
    }
    public string ReadData()
    {
        using (StreamReader reader = new StreamReader("data.txt"))
        {
            string text = reader.ReadToEnd();
            return text;
        }

    }

    public void AddBitDialog(string bit)
    {
        Dialog += bit + '\n'; // Enter
    }
}
