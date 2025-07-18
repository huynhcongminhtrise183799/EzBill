﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum PaymentHistoryPaymentType
    {
        SETTLEMENT, TAX_REFUND, ADVANCE_REIMBURSE
    }
    public class PaymentHistory
    {
        public Guid PaymentHistoryId { get; set; }

        public Guid TripId { get; set; }

        public Guid FromAccountId { get; set; }

        public Guid ToAccountId { get; set; }

        public double Amount { get; set; }

        public string? PaymentUrlBill { get; set; }

        public Guid? RelatedTaxRefundId { get; set; }

        public string PaymentType { get; set; }

        public Trip Trip { get; set; }

        public Account FromAccount { get; set; }

        public Account ToAccount { get; set; }

        public TaxRefund TaxRefund { get; set; }
    }
}
