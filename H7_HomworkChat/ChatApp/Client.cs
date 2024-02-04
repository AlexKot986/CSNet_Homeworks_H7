using ChatCommand;
using ChatNetwork;

namespace ChatApp
{
    public class Client<T>
    {
        string name;
        IMessageSourceClient<T> messageSource;
        public Client(string name, IMessageSourceClient<T> messageSource)
        {
            this.name = name;
            this.messageSource = messageSource;
        }


        void ClientListener()
        {
            T remoteEndPoint = messageSource.CreateNewT();

            while (true)
            {
                try
                {
                    var messageReceived = messageSource.Receive(ref remoteEndPoint);

                    Console.WriteLine($"Получено сообщение от {messageReceived.FromName}:");
                    Console.WriteLine(messageReceived.Text);

                    Confirm(messageReceived);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении сообщения: " + ex.StackTrace);
                    throw new Exception("----------------");
                }
            }
        }

        void Confirm(ChatMessage message)
        {
            var messageAnswer = new ChatMessage() { FromName = name, ToName = "Server", Text = "Conafirm", Id = message.Id, Command = Command.Confirmation };
            messageSource.Send(messageAnswer);
        }


        void Register()
        {
            var chatMessage = new ChatMessage() { FromName = name, ToName = "Server", Text = string.Empty, Command = Command.Register };

            messageSource.Send(chatMessage);
        }

        void ClientSender()
        {

            Register();

            while (true)
            {
                try
                {
                    Console.WriteLine("UDP Клиент ожидает ввода сообщения");

                    Console.Write("Введите  имя получателя и сообщение и нажмите Enter: ");
                    var messages = Console.ReadLine().Split(' ');

                    var message = new ChatMessage() { Command = Command.Message, FromName = name, ToName = messages[0], Text = messages[1] };

                    messageSource.Send(message);
                    Console.WriteLine("Сообщение отправлено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.StackTrace);
                }
            }
        }

        public void Start()
        {
            Register();
            Console.WriteLine("CLient!!!");

            new Thread(() => ClientListener()).Start();

            ClientSender();
        }

    }
}
