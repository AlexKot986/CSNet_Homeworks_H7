using ChatCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNetwork
{
    public interface IMessageSourceClient<T>
    {
        public void Send(ChatMessage message);
        public ChatMessage Receive(ref T ep);
        public T GetAddress();

        public T CreateNewT();
       
    }
}
