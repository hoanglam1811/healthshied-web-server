namespace PaymentService.Entities
{
	public class Payment
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int AppointmentId { get; set; }
		public decimal Amount { get; set; }
		public string Status{ get; set; }
		public PaymentMethod PaymentMethod { get; set; }

	}

	public enum PaymentMethod
	{
		BANK_TRANSFER,
		CASH
	}

}
