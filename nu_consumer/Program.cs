using Confluent.Kafka;
using System;
using System.Threading;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace nu_consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "127.0.0.1:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var c = new ConsumerBuilder<Ignore, string>(conf).Build();            
            c.Subscribe("nu-topic");
            var cts = new CancellationTokenSource();
            
            Console.CancelKeyPress += (_, e) => {
                c.Unsubscribe();
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = c.Consume(cts.Token);
                        if(!string.IsNullOrEmpty(cr.Message.Value))
                            ExecuteOperation(cr.Message.Value);                        
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                c.Close();
            }
            
        }

        static void ExecuteOperation (string msg) {
            var jsonType = JsonHelper(msg);
            Console.WriteLine(jsonType);
        }

        static string JsonHelper(string value){
            var index = value.IndexOf(':');
            var jsonWithoutObjectName = value.Substring(index + 1);
            var jsonWithoutObjectNameRemoveLastChar = jsonWithoutObjectName.Remove(jsonWithoutObjectName.Length - 1);
            
            return jsonWithoutObjectNameRemoveLastChar;
        }
    }
}