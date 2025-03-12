namespace PaymentService.Entities
{
	public class Notification
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Message { get; set; }
		public DateTime SentAt { get; set; }
		public string Status { get; set; }
	}
}
