using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


Console.WriteLine(" [*] Waiting for messages.");

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

var randomQueueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new()
{
    { "f3ormat", "pdf" },
    { "shape", "a4" },
    { "x-match", "any" }
};

channel.QueueBind(randomQueueName, "header-exchange", string.Empty, headers);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received: {message}");
    //Bu işlemden sonra kuyruktaki veriyi sil
    Thread.Sleep(250);
    channel.BasicAck(e.DeliveryTag, false);
};

channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
