using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System.Data;
using System.Text;
using System.Threading.Channels;

internal class Program
{
    private static async Task Main(string[] args)
    {

        // FANOUT EXCHANGE ÖRNEĞİ
        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName = "localhost";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";



        //!!! excahge oluşturmayı da publisher yaparken bazı durumlarda  Kuyruk oluşturmayı  consumer da yapabilir. Ancak bu durum istisna ve dikkatli olmak lazım.
        // Fanout Exchange kendisine bağlı olan tüm kuyruklara aynı mesajı iletir.
        // Kaç tane consumer bağlanacağını bilmediğimiz durumlarda burada kullanıcılara kendi kuyruklarınızı oluşturup hava durumlarını çekebilirsiniz. Biz saate bir Fanout exchange ile bir kanal oluşturuyoruz buradan isteyen alsın diyebilirsin.
        // Kullanmak istemeyen kuyruğunu kapatsın gitsin.

        // Fanout da genelde consumer lar kendi kuyruklarını gönderiyor. Önceki gönderdiklerimin önemi olmadığından kuyruğunu oluşturup bağlanan gelip yeni tahminleri almaya başlasın.

        // 








        IConnection con = factory.CreateConnectionAsync().Result;// connection oluştur
        IChannel channel = con.CreateChannelAsync().Result;   // rabbitmq ya bir kanal oluştur

        //channel.QueueDeclareAsync("RabbitMQSinan", true, false, false,null); // Burayı silip exchange oluşturalım
        // true yaptık uygulama reset attığında gitmesin,
        await channel.ExchangeDeclareAsync("fanoutexchange_log", ExchangeType.Fanout, true, false, null);




        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            string mesaj = $"Merhaba Rabbit Ben bir logum {x} ";
            byte[] mesajbody = Encoding.UTF8.GetBytes(mesaj); // RabitMQ byte[] dizisi alır.
            channel.BasicPublishAsync("fanoutexchange_log", "", mesajbody); // Bureada artık excahnge ismini veriyoruz.

            Console.WriteLine("Mesaj Gönderildi : " + mesaj);
        });







        Console.ReadLine();
    }
}