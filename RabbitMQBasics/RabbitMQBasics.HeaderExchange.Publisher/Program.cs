using RabbitMQ.Client;
using System.Reflection.PortableExecutable;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", ExchangeType.Headers, true, false);

Dictionary<string, object> headers = new()
{
    { "format", "pdf" },
    { "shape", "a4" }
};

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

foreach (var item in Enumerable.Range(1, 100))
{
    string message = $"Message {item}";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "header-exchange", routingKey: string.Empty, basicProperties: properties, body: body);

    Console.WriteLine($"[x] Sent {message}");
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
