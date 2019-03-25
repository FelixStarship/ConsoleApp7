using RabbitMQ.Client;
using System;
using System.Text;


namespace Send3
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",//IP地址
                Port = 5672,//端口号
                UserName = "admin",//用户账号
                Password = "admin123",//用户密码
                VirtualHost = "TestRabbitMq"
            };

            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = "cavalier_test";

                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                    for (int i = 0; i < 10; i++)
                    {

                        String message = "test push ....." + i.ToString();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        IBasicProperties props = channel.CreateBasicProperties();
                        props.ContentType = "text / plain";
                        props.DeliveryMode = 2;
                        props.Expiration = "3600000";

                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: props, body: body);

                        Console.WriteLine($"消息:{message}");
                    }
                }
            }
        }
        
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}