using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {


        await Hadi();

        Console.ReadLine();
    }
    public static async Task Hadi()
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName = "localhost";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";

        using IConnection con = await factory.CreateConnectionAsync();// connection oluştur
        using IChannel channel = await con.CreateChannelAsync();   // rabbitmq ya bir kanal oluştur

       await channel.BasicQosAsync(0, 1, false); // Her bir subscribe 5 er 5 er gönder  //  true yaparsak tüm subscriberlar için 5 adet olacak şekilde böl demiş oluyoruz.
        


        //    channel.QueueDeclareAsync("RabbitMQSinan", true, false, false); // silinebilir de. Eminsek kuyruk olduğunda
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += Consumer_ReceivedAsync;
        await channel.BasicConsumeAsync("RabbitMQSinan", autoAck: false, consumer: consumer); //autoAck: Bunu false yaparsak işlem bitince biz sil diyebileceğiz
        Console.ReadLine();

    }

    private static Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        var body = @event.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Thread.Sleep(2000);


        Console.WriteLine($" [x] Received {message}");
        ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(@event.DeliveryTag, false); // Hangi tagla buraya gelmişse. O taglı kuyruğu sil, false ise Diğerlerini silsin mi, hayır




        return Task.CompletedTask;
    }
}