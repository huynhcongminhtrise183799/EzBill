using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using EzBill.Models.Request.Payment;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Payment;
using EzBill.Application.ServiceModel.AccountSubscriptions;

namespace EzBill.Controllers
{
	[ApiController]
	[Route("api/payment/")]
	public class PaymentController : ControllerBase
	{
		private readonly PayOS _payOS;
		private readonly IPaymentHistoryService _paymentHistoryService;
		private readonly IAccountSubscriptionsService _service;
		private const string COMPLEDTED_PAYMENT = "COMPLETED";
		private const string FAILED_PAYMENT = "FAILED";

		public PaymentController(PayOS payOS, IPaymentHistoryService paymentHistoryService, IAccountSubscriptionsService service)
		{
			_payOS = payOS;
			_paymentHistoryService = paymentHistoryService;
			_service = service;
		}

		[HttpPost("buy-plan")]
		public async Task<IActionResult> BuyPlan([FromBody] CreatePaymentRequest request)
		{
			

			// Tạo mã đơn hàng (unique)
			long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			var items = new List<ItemData>
			{
				new ItemData(request.PlanName, 1, (int)request.Price)
			};

			var paymentData = new PaymentData(
			orderCode,
			(int)request.Price,
			"Thanh toán gói: " + request.PlanName,
			items,
			"ezbill://payment-cancel",  
			"ezbill://payment-success"   
			);


			try
			{
				var model = new CreatePaymentHistoryModel
				{
					PaymentHistoryModelId = Guid.NewGuid(),
					FromAccountId = request.AccountId,
					Amount = request.Price,
					OrderCode = orderCode,
					PlanId = request.PlanId
				};
				var result = await _paymentHistoryService.AddPaymentHistoryAsync(model);
				if (!result)
				{
					return BadRequest(new { error = "Tạo lịch sử thanh toán thất bại" });
				}
				var createResult = await _payOS.createPaymentLink(paymentData);

				return Ok(new
				{
					paymentUrl = createResult.checkoutUrl,
					orderCode = orderCode
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPost("confirm-webhook")]
		public async Task<IActionResult> ConfirmWebhook([FromBody] ConfirmWebhook body)
		{
			try
			{
				await _payOS.confirmWebhook(body.Webhook_url);
				return Ok(new { code = 0, message = "Webhook confirmed" });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return BadRequest(new { code = -1, message = "Confirm webhook fail" });
			}
		}

		

		[HttpPost("webhook")]
		public async Task<IActionResult> Webhook([FromBody] WebhookType webhookBody)
		{
			try
			{
			// verifyPaymentWebhookData trả về WebhookData (những trường: orderCode, amount, description, ...)
				WebhookData webhookData = _payOS.verifyPaymentWebhookData(webhookBody);

			// Bạn có thể kiểm tra success ở phần gốc (webhookBody) hoặc kiểm tra webhookData.code == "00"
				if (webhookBody != null && webhookBody.success)
				{
				// truy cập trực tiếp các trường trong WebhookData (camelCase)
					var orderCode = webhookData.orderCode;   // long

					// Cập nhật trạng thái thanh toán trong hệ thống của bạn
					
					var payment = await _paymentHistoryService.GetByOrderCode(orderCode);
					if (payment == null)
					{
						// Log lỗi không tìm thấy đơn hàng hoặc cập nhật thất bại
						return BadRequest(new { code = "-1", message = "Order not found or update failed" });
					}
					var updateResult = await _paymentHistoryService.ChangePaymentStatus(orderCode, COMPLEDTED_PAYMENT);
					var model = new CreateAccountSubscriptionsModel
					{
						AccountId = payment.FromAccountId,
						PlanId = (Guid)payment.PlanId
					};
					var addSub = await _service.AddAccountSubscriptionsAsync(model);
					if (!addSub)
					{
						return BadRequest(new { code = "-1", message = "Add subscription failed" });
					}
				}
				else
				{
					var orderCode = webhookData.orderCode;   

					var updateResult = await _paymentHistoryService.ChangePaymentStatus(orderCode, FAILED_PAYMENT);

				}

				// trả 200 để payOS biết đã nhận
				return Ok(new { code = "00", message = "processed" });
			}
			catch (Exception ex)
			{
			// SDK có thể throw nếu signature không hợp lệ -> log để debug
				Console.WriteLine(ex);
				return BadRequest(new { code = "-1", message = ex.Message });
			}
	}

		[HttpGet("{orderCode}")]
		public async Task<IActionResult> GetPaymentByOrderCode([FromRoute] long orderCode)
		{
			var payment = await _paymentHistoryService.GetPaymentStatusByOrderCode(orderCode);
			return Ok(new
			{
				status = payment
			});
		}	


	}
}
