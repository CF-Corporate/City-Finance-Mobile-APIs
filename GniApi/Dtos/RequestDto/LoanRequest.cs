namespace GniApi.Dtos.RequestDto
{
    public class Loanrequest
    {
        public int amount { get; set; }
        public int productCode { get; set; }
        public bool limitExceeded { get; set; }
        public int percentageRate { get; set; }
        public int comissionPercent { get; set; }
        public int repaymentAmount { get; set; }
        public int repaymentPeriod { get; set; }
    }
}
