using ChatCommand;
using ChatNetwork;
using Contexts.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    public class Server<T>
    {
        public Dictionary<string, T> clients = new Dictionary<string, T>();

        IMessageSourceServer<T> messageSource;

        public Server(IMessageSourceServer<T> source)
        {
            messageSource = source;
        }

    
        void Register(ChatMessage message)
        {
            Console.WriteLine("Message Register, name = " + message.FromName + "  " + message.Text);
            if (clients.TryAdd(message.FromName, messageSource.GetAddress()))
                Console.WriteLine("Пользователь зарегистрирован");
            else
                Console.WriteLine("Зарегистрировать пользователя не удалось");

            using (var ctx = new Context())
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) != null) return;

                ctx.Add(new User { Name = message.FromName });

                ctx.SaveChanges();
            }
        }

        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);

            using (var ctx = new Context())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.Id == id);

                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
        }

        void RelyMessage(ChatMessage message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out T ep))
            {
                using (var ctx = new Context())
                {
                    var fromUser = ctx.Users.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users.First(x => x.Name == message.ToName);
                    var msg = new Message { FromUser = fromUser, ToUser = toUser, Received = false, Text = message.Text };
                    ctx.Messages.Add(msg);

                    ctx.SaveChanges();

                    id = msg.Id;
                }


                var forwardMessage = new ChatMessage() { Id = id, Command = Command.Message, ToName = message.ToName, FromName = message.FromName, Text = message.Text };

                messageSource.Send(forwardMessage, ep);

                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        void ProcessMessage(ChatMessage message)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
            Console.WriteLine(message.Text);


            if (message.Command == Command.Register)
            {
                Register(message);

            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }

       




        public void Work()
        {
            Console.WriteLine("UDP Клиент ожидает сообщений...");

            while (true)
            {
                try
                {
                    T remoteEndPoint = messageSource.CreateNewT();
                    var message = messageSource.Receive();

                    if (message == null)
                    {
                        Console.WriteLine("Message is null!");
                        return;
                    }

                    ProcessMessage(message);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex);
                    return;
                }

            }
        }

    }
}
