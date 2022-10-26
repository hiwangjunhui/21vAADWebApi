// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;

var conf = new ProducerConfig { BootstrapServers = "192.168.0.102:9092"};

Action<DeliveryReport<string, string>> handler = r =>
    Console.WriteLine(!r.Error.IsError
        ? $"Delivered message to {r.TopicPartitionOffset}, partition: {r.Partition}, {r.Message.Value}"
        : $"Delivery Error: {r.Error.Reason}");

using (var p = new ProducerBuilder<string, string>(conf).Build())
{
    for (int i = 0; i < 1000000; i++)
    {
        p.Produce("test", new Message<string, string> { Value = $"王军辉  {i+1}"}, handler);
        if (i % 500 == 0)
        {
            p.Flush();
        }
    }
    p.Flush();
}
