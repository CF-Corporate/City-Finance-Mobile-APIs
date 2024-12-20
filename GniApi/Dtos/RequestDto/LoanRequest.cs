﻿using System.Text.Json.Serialization;

namespace GniApi.Dtos.RequestDto
{
    public class Loanrequest
    {
        public decimal amount { get; set; }
        public int productCode { get; set; }
        [JsonIgnore]
        public bool? limitExceeded { get; set; }
        public int percentageRate { get; set; }
        public int comissionPercent { get; set; }
        public decimal repaymentAmount { get; set; }
        public int repaymentPeriod { get; set; }
    }
}
