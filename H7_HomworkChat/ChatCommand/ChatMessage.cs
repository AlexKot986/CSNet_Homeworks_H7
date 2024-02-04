using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatCommand
{
    public class ChatMessage
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static ChatMessage FromJson(string json)
        {
            return JsonSerializer.Deserialize<ChatMessage>(json);
        }

        public override string ToString()
        {
            return $"Сообщение от {FromName}: {Text}";
        }
    }
}
