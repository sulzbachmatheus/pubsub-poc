using System;
using System.Threading;

namespace nu_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console App Running!");

            Thread threadProducer = new Thread(new ThreadStart(Producer.Run));
            threadProducer.Start();

            Thread threadConsumer = new Thread(new ThreadStart(Consumer.Run));
            threadConsumer.Start();
        }
    }
}
