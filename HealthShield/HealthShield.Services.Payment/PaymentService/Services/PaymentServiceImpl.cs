using Grpc.Core;
using Payments;
using PaymentService.DbContexts;
using PaymentService.Entities;

namespace PaymentService.Services
{
	public class PaymentServiceImpl : Payments.PaymentService.PaymentServiceBase
	{
		private readonly PaymentDbContext _dbContext;

		public PaymentServiceImpl(PaymentDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public override async Task<PaymentResponse> CreatePayment(CreatePaymentRequest request, ServerCallContext context)
		{
			var payment = new Payment
			{
				UserId = request.UserId,
				AppointmentId = request.AppointmentId,
				Amount = (decimal)request.Amount,
				Status = request.Status,
				PaymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod, true)
			};

			_dbContext.Payments.Add(payment);
			await _dbContext.SaveChangesAsync();

			return new PaymentResponse
			{
				Id = payment.Id,
				UserId = payment.UserId,
				AppointmentId = payment.AppointmentId,
				Amount = (double)payment.Amount,
				Status = payment.Status,
				PaymentMethod = payment.PaymentMethod.ToString()
			};
		}

		public override async Task<PaymentResponse> GetPaymentById(PaymentRequest request, ServerCallContext context)
		{
			var payment = await _dbContext.Payments.FindAsync(request.PaymentId);

			if (payment == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Payment not found"));

			return new PaymentResponse
			{
				Id = payment.Id,
				UserId = payment.UserId,
				AppointmentId = payment.AppointmentId,
				Amount = (double)payment.Amount,
				Status = payment.Status,
				PaymentMethod = payment.PaymentMethod.ToString()
			};
		}
	}
}
