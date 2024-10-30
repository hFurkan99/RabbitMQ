using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.ExchangeDeclare("logs-direct", ExchangeType.Direct, true);

foreach (var logType in Enum.GetNames(typeof(LogNames)).ToList())
{
    var routeKey = $"route-{logType}";
    var queueName = $"direct-queue-{logType}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct", routeKey);
}
foreach (var item in Enumerable.Range(1, 100))
{
    LogNames logType = (LogNames)new Random().Next(1, 5);
    string message = $"log {item} --- log-type: {logType}";

    var body = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{logType}";

    channel.BasicPublish(exchange: "logs-direct", routeKey , basicProperties: null, body: body);

    Console.WriteLine($"[x] Sent {message}");
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
