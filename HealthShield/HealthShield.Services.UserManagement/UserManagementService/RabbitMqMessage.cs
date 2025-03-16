public class RabbitMqMessage<T>
{
    public string Action { get; set; }  // "Create", "Update", "Delete"
    public string Entity { get; set; }  // "User", "Order"
    public T Data { get; set; }         // Actual data payload
}
