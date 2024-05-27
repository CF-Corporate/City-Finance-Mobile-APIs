namespace GniApi.Dtos.RequestDto
{
    public class Loanrequest
    {
        public double amount { get; set; }
        public double amountAfterCommission { get; set; }
        public int productCode { get; set; }
        public bool limitExceeded { get; set; }
        public double percentageRate { get; set; }
        public double comissionPercent { get; set; }
        public double repaymentAmount { get; set; }
        public int repaymentPeriod { get; set; }
    }
}
