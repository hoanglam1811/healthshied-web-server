using System.Text;
using System.Text.Json;
using Payments;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumer
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqConsumer()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        // Declare queue (make sure it exists)
        _channel.QueueDeclareAsync("paymentQueue", durable: true, exclusive: false).GetAwaiter().GetResult();
        _channel.QueueBindAsync("paymentQueue", "paymentExchange", "").GetAwaiter().GetResult();
    }

    public async Task StartConsuming()
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<RabbitMqMessage<JsonElement>>(messageJson);

            if (message != null)
            {
                Console.WriteLine($"📥 Received {message.Action} for {message.Entity}");

                if (message.Entity == "User" && message.Action == "Create")
                {
                    await HandleCreateUser(message.Data);
                }
            }

            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await _channel.BasicConsumeAsync(queue: "paymentQueue", autoAck: false, consumer: consumer);
        Console.WriteLine("🟢 Consumer started. Listening for messages...");
    }

    private async Task HandleCreateUser(JsonElement data)
    {
        var user = JsonSerializer.Deserialize<CreatePaymentRequest>(data);
        Console.WriteLine($"👤 Creating User: {user.Amount}, Email: {user.PaymentMethod}");

        // Process the user (e.g., store in DB or call gRPC)
        await Task.CompletedTask;
    }
}
