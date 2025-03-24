
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Users;

public class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string PaymentExchange = "paymentExchange";
    private const string UserExchange = "userExchange";
    private const string VaccinationExchange = "vaccinationExchange";

    public RabbitMqPublisher(IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig["Host"] ?? "",
            UserName = rabbitMqConfig["User"] ?? "",
            Password = rabbitMqConfig["Password"] ?? ""
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(exchange: PaymentExchange, type: ExchangeType.Fanout).GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(exchange: UserExchange, type: ExchangeType.Fanout).GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(exchange: VaccinationExchange, type: ExchangeType.Fanout).GetAwaiter().GetResult();
    }

    public async Task PublishMessageAsync<T>(RabbitMqMessage<T> message, string exchangeName, string routingKey = "", CancellationToken cancellationToken = default)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var properties = new BasicProperties();

        await _channel.BasicPublishAsync<BasicProperties>(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken
        );

        Console.WriteLine($"Published Message: {jsonMessage}");
    }

    public void Dispose()
    {
        _channel?.CloseAsync(CancellationToken.None).GetAwaiter().GetResult();
        _channel?.DisposeAsync().AsTask().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
        _connection?.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}

