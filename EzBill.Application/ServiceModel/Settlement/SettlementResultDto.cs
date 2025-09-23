using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Settlement
{
    public class SettlementResultDto
    {
        public Guid SettlementId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string TripName { get; set; }
    }

    public class BudgetReportDto
    {
        public string AccountName { get; set; }
        public double RemainingBudget { get; set; }
    }

    public class GenerateSettlementResponse
    {
        public List<SettlementResultDto> Settlements { get; set; }
        public List<BudgetReportDto> BudgetReports { get; set; }
    }
}
