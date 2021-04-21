using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

class Producer {

    public static void Run() {

        var config = new ProducerConfig { BootstrapServers = "127.0.0.1:9092" };

        using var p = new ProducerBuilder<Null, string>(config).Build();

        {
            try
            {
                var count = 0;
                while (true)
                {
                    var dr = Task.Run(() => p.ProduceAsync("test-topic", 
                        new Message<Null, string> { Value = $"test: {count++}" })).Result;

                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset} | {count}'");

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