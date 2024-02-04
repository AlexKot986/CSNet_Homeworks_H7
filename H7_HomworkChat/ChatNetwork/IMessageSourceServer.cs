using ChatCommand;

namespace ChatNetwork
{
    public interface IMessageSourceServer<T>
    {
        T GetAddress();
        void Send(ChatMessage message, T ep);
        ChatMessage Receive();

        T CreateNewT();
    }
}
