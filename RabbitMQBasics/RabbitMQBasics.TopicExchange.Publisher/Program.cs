using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.ExchangeDeclare("logs-topic", ExchangeType.Topic, true);


Random random = new();

foreach (var item in Enumerable.Range(1, 100))
{

    LogNames logType1 = (LogNames)random.Next(1, 5);
    LogNames logType2 = (LogNames)random.Next(1, 5);
    LogNames logType3 = (LogNames)random.Next(1, 5);

    var routeKey = $"{logType1}.{logType2}.{logType3}";
    string message = $"log type: {logType1}-{logType2}-{logType3}";

    var body = Encoding.UTF8.GetBytes(message);


    channel.BasicPublish(exchange: "logs-topic", routeKey, basicProperties: null, body: body);

    Console.WriteLine($"[x] Sent {message}");
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
