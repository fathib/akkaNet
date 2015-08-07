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
    public class Extract : TypedActor, IHandle<ETLMessage>
    {
        private int count;
        IActorRef childActorTransform;
        public Extract()
        {
            childActorTransform = Context.ActorOf(Props.Create<Transform>());
        }

        public void Handle(ETLMessage message)
        {
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
    public class ExtractActor : TypedActor, IHandle<ETLMessage>
    {
        IActorRef childActorTransform;
        IActorRef childActorError;
        public ExtractActor()
        {
            childActorTransform = Context.ActorOf(Props.Create<TransformActor>());
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
            var newMessage = string.Format("> EXTRACT :#{0} -- {1}", message.DataId, message.Message);
            Console.WriteLine(newMessage);
            childActorTransform.Tell(message);

        }
    }


    public class TransformActor : TypedActor, IHandle<ETLMessage>
    {
        public void Handle(ETLMessage message)
        {
            Thread.Sleep(6000);
            var newMessage = string.Format(">>>>>> TRANSFORM :#{0} -- {1}", message.DataId, message.Message);
            Console.WriteLine(newMessage);
        }
    }


    public class ErrorActor : TypedActor, IHandle<ETLMessage>
    {
        public void Handle(ETLMessage message)
        {
            var newMessage = string.Format(">>>>>> IN ERROR :#{0} -- {1}", message.DataId, message.Message);

            Console.WriteLine(newMessage);

        }
    }


    public class ETLMessage
    {
        public ETLMessage(string message, int dataId)
        {
            Message = message;
            DataId = dataId;
        }

        public string Message { get; private set; }
        public int DataId { get; private set; }
    }

}