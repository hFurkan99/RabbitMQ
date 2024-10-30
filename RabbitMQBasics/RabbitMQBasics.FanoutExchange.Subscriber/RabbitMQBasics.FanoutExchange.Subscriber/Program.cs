using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


Console.WriteLine(" [*] Waiting for messages.");

//var randomQueueName = channel.QueueDeclare().QueueName;
//Consumer kapansa da kuyruğun kalması ve kaldığı yerden devam etmesi için queuename sabit olmalı
var randomQueueName = "log-database-save-queue";
// ve aşağıdaki satır eklenmeli
channel.QueueDeclare(randomQueueName, true, false, false);

channel.QueueBind(randomQueueName, "logs-fanout", string.Empty);
channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received: {message}");
    Thread.Sleep(1000);
    //Bu işlemden sonra kuyruktaki veriyi sil
    channel.BasicAck(e.DeliveryTag, false);
};

channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
