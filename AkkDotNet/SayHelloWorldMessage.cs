using Akka.Actor;
using System;
using System.Threading;

namespace AkkDotNetV1
{
    public class Extract : TypedActor, IHandle<ETLMessage>
    {
        private int count;
        public void Handle(ETLMessage message)
        {
            Thread.Sleep(1000);
            count++;
            Console.WriteLine("> EXTRACT :" + count + "  " + message.Message);
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

namespace AkkDotNetV2
{
    public class Extract : TypedActor, IHandle<AkkDotNet.ETLMessage>
    {
        private int count;
        IActorRef childActorTransform;
        public Extract()
        {
            childActorTransform = Context.ActorOf(Props.Create<AkkDotNet.Transform>());
        }

        public void Handle(AkkDotNet.ETLMessage message)
        {
            Thread.Sleep(3000);
            count++;
            var newMessage = string.Format("> EXTRACT :{0}  {1}", count, message.Message);
            Console.WriteLine(newMessage);
            childActorTransform.Tell(message);

        }
    }


    public class Transform : TypedActor, IHandle<AkkDotNet.ETLMessage>
    {
        private int count;
        public void Handle(AkkDotNet.ETLMessage message)
        {
            Thread.Sleep(6000);
            count++;
            var newMessage = string.Format(">>>>>> TRANSFORM :{0}  {1}", count, message.Message);
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


namespace  AkkDotNetV3
{
    public class Extract : TypedActor, IHandle<AkkDotNet.ETLMessage>
    {
        private int count;
        IActorRef childActorTransform;
        IActorRef childActorError;
        public Extract()
        {
            childActorTransform = Context.ActorOf(Props.Create<AkkDotNet.Transform>());
            childActorError = Context.ActorOf(Props.Create<AkkDotNet.ErrorActor>());
        }

        public void Handle(AkkDotNet.ETLMessage message)
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


    public class Transform : TypedActor, IHandle<AkkDotNet.ETLMessage>
    {
        private int count;
        public void Handle(AkkDotNet.ETLMessage message)
        {
            Thread.Sleep(6000);
            count++;
            var newMessage = string.Format(">>>>>> TRANSFORM :{0}  {1}", count, message.Message);
            Console.WriteLine(newMessage);
        }
    }


    public class ErrorActor : TypedActor, IHandle<AkkDotNet.ETLMessage>
    {
        private int count;
        public void Handle(AkkDotNet.ETLMessage message)
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