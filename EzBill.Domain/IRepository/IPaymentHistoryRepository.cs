using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IPaymentHistoryRepository
	{
		Task<bool> AddPaymentHistoryAsync(PaymentHistory payment);

		Task<bool> ChangePaymentStatus(long OrderCode, string status);

		Task<PaymentHistory?> GetByOrderCode(long OrderCode);
	}
}
