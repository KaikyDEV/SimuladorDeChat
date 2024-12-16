using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

class Program
{
    static ConcurrentQueue<string> chatMessages = new ConcurrentQueue<string>();
    static bool chatActive = true;

    static void Main(string[] args)
    {
        Console.WriteLine("Bem-vindo ao chat!");

        Thread chatDisplayThread = new Thread(DisplayChatMessages);
        chatDisplayThread.Start();

        Thread user1Thread = new Thread(() => SimulateUserChat("Usuário1"));
        Thread user2Thread = new Thread(() => SimulateUserChat("Usuário2"));
        Thread user3Thread = new Thread(() => SimulateUserChat("Usuário3"));

        user1Thread.Start();
        user2Thread.Start();
        user3Thread.Start();

        Thread.Sleep(10000); 
        chatActive = false;  

        user1Thread.Join();
        user2Thread.Join();
        user3Thread.Join();
        chatDisplayThread.Join();

        Console.WriteLine("Chat finalizado.");
    }
    static void SimulateUserChat(string userName)
    {
        Random rand = new Random();
        while (chatActive)
        {
            Thread.Sleep(rand.Next(500, 1500));
            string message = $"{userName}: Mensagem {rand.Next(1, 100)}";
            chatMessages.Enqueue(message);
        }
    }
    static void DisplayChatMessages()
    {
        while (chatActive)
        {
            if (!chatMessages.IsEmpty)
            {
                if (chatMessages.TryDequeue(out string message))
                {
                    Console.Clear();
                    Console.WriteLine("=== Chat ===");
                    foreach (var msg in chatMessages)
                    {
                        Console.WriteLine(msg);
                    }
                }
            }
            Thread.Sleep(100);
        }
    }
}
