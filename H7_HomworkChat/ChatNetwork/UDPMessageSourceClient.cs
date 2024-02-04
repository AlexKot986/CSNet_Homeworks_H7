using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatCommand;

namespace ChatNetwork
{
    public class UDPMessageSourceClient : IMessageSourceClient<IPEndPoint>
    {
        private UdpClient client;
        private IPEndPoint endPoint;
        private IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        public UDPMessageSourceClient(int port)
        {
            client = new UdpClient(port);
            endPoint = new IPEndPoint(IPAddress.Any, port);

        }

        public IPEndPoint CreateNewT()
        {
            return serverEndPoint;
        }

        public IPEndPoint GetAddress()
        {
            return endPoint;
        }

        public ChatMessage Receive(ref IPEndPoint remoteEndPoint)
        {
            byte[] receiveBytes = client.Receive(ref remoteEndPoint);
            string receivedData = Encoding.ASCII.GetString(receiveBytes);
            var messageReceived = ChatMessage.FromJson(receivedData);

            return messageReceived;
        }

        public void Send(ChatMessage message)
        {
            var json = message.ToJson();
            var b = Encoding.ASCII.GetBytes(json);
            client.Send(b, serverEndPoint);
        }

    }
}
