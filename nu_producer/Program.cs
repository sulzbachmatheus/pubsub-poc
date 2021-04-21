using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nu_producer
{
    class Program
    {
        public static async Task Main()
        {
            var config = new ProducerConfig { BootstrapServers = "127.0.0.1:9092" };

            using var p = new ProducerBuilder<Null, string>(config).Build();

            {
                try
                {
                    while (true)
                    {
                        var payload = Console.ReadLine();

                        var dr = await p.ProduceAsync("test-topic", 
                            new Message<Null, string> { Value = $"test: {payload}" });

                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset} | {payload}'");

                        Thread.Sleep(2000);
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}