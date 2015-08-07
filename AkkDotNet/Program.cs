using Akka.Actor;
using System;
using System.Threading;
using AkkDotNetV3;

namespace AkkDotNet
{
    class Program
    {
static void Main(string[] args)
{
    using (var system = ActorSystem.Create("ETL"))
    {
        IActorRef extractActor = system.ActorOf(Props.Create<ExtractActor>());

        var count = 0;
        Console.WriteLine("Messages à traiter:");
        while (true)
        {
            count++;
            string line = Console.ReadLine();
            var message = new ETLMessage(line, count);
            extractActor.Tell(message);
        }
    }
}
    }

    public class Ping : TypedActor, IHandle<PingPongCounterMessage>
    {
        
        IActorRef children;
        public Ping()
        {
            children = Context.ActorOf(Props.Create<Pong>());
        }

        public void Handle(PingPongCounterMessage message)
        {
            Thread.Sleep(1000);
            
            Console.WriteLine("ping"+ message.Count);
            children.Tell(new PingPongCounterMessage(message.Count + 1));
        }
    }


    public class Pong : TypedActor, IHandle<PingPongCounterMessage>
    {
        
        public void Handle(PingPongCounterMessage message)
        {
            Thread.Sleep(1000);
            
            Console.WriteLine("pong"+message.Count);
            Sender.Tell(new PingPongCounterMessage(message.Count+1));
        }
    }

    

    public class PingPongCounterMessage
    {
        public PingPongCounterMessage(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }
        
    }

}
