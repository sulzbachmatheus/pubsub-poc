using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
            
            try
            {
                Console.WriteLine("Post the operations to be consumed and press enter in an empty line to send the content");
                StringBuilder builder = new StringBuilder();
                while (true)
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    {
                        break;
                    }
                    else
                    {
                        builder.Append(input);
                    }
                }
                //removing empty spaces
                var removingStuffs = Regex.Replace(builder.ToString(), @"\s+", string.Empty);
                var replacement = removingStuffs.Replace("}}", "}} ");
                var replacementList = replacement.Split(' ');

                foreach(var item in replacementList) {
                    var dr = await p.ProduceAsync("nu-topic", new Message<Null, string> { Value = item });
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