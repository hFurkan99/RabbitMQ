using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, true);

foreach (var item in Enumerable.Range(1, 100))
{
    string message = $"log {item}";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "logs-fanout", string.Empty, basicProperties: null, body: body);

    Console.WriteLine($"[x] Sent {message}");
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
