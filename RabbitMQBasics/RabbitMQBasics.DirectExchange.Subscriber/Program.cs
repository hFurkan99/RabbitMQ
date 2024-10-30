using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


Console.WriteLine(" [*] Waiting for messages.");

var queueName = "direct-queue-Critical";

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received: {message}");
    Thread.Sleep(200);

    File.AppendAllText("log-critical.txt", message + "\n");

    //Bu işlemden sonra kuyruktaki veriyi sil
    channel.BasicAck(e.DeliveryTag, false);
};

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
