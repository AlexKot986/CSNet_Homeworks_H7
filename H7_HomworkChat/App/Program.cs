using ChatApp;
using ChatNetwork;
using System.Net;

Dictionary<string, IPEndPoint> dict = new Dictionary<string, IPEndPoint>()
{
    {"Mike", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55501) },
    {"John", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55502) }
};


if (args.Length == 0)
{
    var s = new Server<IPEndPoint>(new UDPMessageSourceServer());
    s.clients = dict;
    s.Work();
}
else
if (args.Length == 2)
{
    var c = new Client<IPEndPoint>(args[0], new UDPMessageSourceClient(int.Parse(args[1])));
    c.Start();
}

else
{
    Console.WriteLine("Для запуска клиента введите ник-нейм и порт как параметры запуска приложения");
}
