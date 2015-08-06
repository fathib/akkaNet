using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("ETL"))
            {
                IActorRef extractActor = system.ActorOf(Props.Create<Extract>());


                Console.WriteLine("Messages à traiter:");
                while (true)
                {
                    string line = Console.ReadLine();
                    extractActor.Tell(new ETLMessage(line));
                }
            }
        }
    }

    public class Extract : TypedActor, IHandle<ETLMessage>
    {
        private int count;
        IActorRef childActorTransform;
        IActorRef childActorError;
        public Extract()
        {
            childActorTransform = Context.ActorOf(Props.Create<Transform>());
            childActorError = Context.ActorOf(Props.Create<ErrorActor>());
        }

        public void Handle(ETLMessage message)
        {
            if (message.Message.StartsWith("a"))
            {
                childActorError.Tell(message);
                return;
            }
            Thread.Sleep(3000);
            count++;
            var newMessage = string.Format("> EXTRACT :{0}  {1}", count, message.Message);
            Console.WriteLine(newMessage);
            childActorTransform.Tell(message);

        }
    }


    public class Transform : TypedActor, IHandle<ETLMessage>
    {
        private int count;
        public void Handle(ETLMessage message)
        {
            Thread.Sleep(6000);
            count++;
            var newMessage = string.Format(">>>>>> TRANSFORM :{0}  {1}", count, message.Message);
            Console.WriteLine(newMessage);
        }
    }


    public class ErrorActor : TypedActor, IHandle<ETLMessage>
    {
        private int count;
        public void Handle(ETLMessage message)
        {
            count++;
            var newMessage = string.Format(">>>>>> IN ERROR :{0}  {1}", count, message.Message);
            
            Console.WriteLine(newMessage);
            
        }
    }


    public class ETLMessage
    {
        public ETLMessage(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
        
    }

}
