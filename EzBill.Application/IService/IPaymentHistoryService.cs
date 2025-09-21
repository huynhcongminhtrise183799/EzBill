using EzBill.Application.ServiceModel.Payment;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IPaymentHistoryService
	{
		Task<bool> AddPaymentHistoryAsync(CreatePaymentHistoryModel payment);
		Task<bool> ChangePaymentStatus(long OrderCode);
		Task<PaymentHistory?> GetByOrderCode(long OrderCode);

	}
}
