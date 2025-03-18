public class RabbitMqMessage<T>
{
    public string Action { get; set; }  // e.g., "Create", "Update", "Delete"
    public string Entity { get; set; }  // e.g., "User", "Order"
    public T Data { get; set; }         // The actual data payload
}
