namespace EzBill.Models.Request.Payment
{
	public class CreatePaymentRequest
	{
		public Guid AccountId { get; set; }
		public Guid PlanId { get; set; }

		public string PlanName { get; set; }
		public double Price { get; set; }

	}
}
