using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "work-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

foreach (var item in Enumerable.Range(1, 100))
{
    string message = $"Message {item}";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty, routingKey: "work-queue", basicProperties: null, body: body);

    Console.WriteLine($"[x] Sent {message}");
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
