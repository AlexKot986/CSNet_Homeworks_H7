using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatCommand;

namespace ChatNetwork
{
    public class UDPMessageSourceServer : IMessageSourceServer<IPEndPoint>
    {
        private UdpClient udpClient;
        private IPEndPoint udpEndPoint;
        public UDPMessageSourceServer()
        {
            udpClient = new UdpClient(12345);
        }
 
        public ChatMessage Receive()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
            string receivedData = Encoding.ASCII.GetString(receiveBytes);

            return ChatMessage.FromJson(receivedData);
        }

       


        public void Send(ChatMessage message, IPEndPoint ep)
        {
            byte[] forwardBytes = Encoding.ASCII.GetBytes(message.ToJson());

            udpClient.Send(forwardBytes, forwardBytes.Length, ep);
        }

        public IPEndPoint GetAddress()
        {
            return udpEndPoint;
        }

        public IPEndPoint CreateNewT()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }
    }
}
