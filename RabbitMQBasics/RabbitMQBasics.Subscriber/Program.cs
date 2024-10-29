using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//channel.QueueDeclare(queue: "work-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received: {message}");
    //Bu işlemden sonra kuyruktaki veriyi sil
    channel.BasicAck(e.DeliveryTag, false);
};

channel.BasicConsume("work-queue", false, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
