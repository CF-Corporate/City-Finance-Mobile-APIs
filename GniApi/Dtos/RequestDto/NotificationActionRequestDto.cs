namespace GniApi.Dtos.RequestDto
{
    public class NotificationActionRequestDto
    {
        public long RequestId { get; set; }

        public string Pin { get; set; }

        [ValueIn("APPROVE_LOAN_REQUEST",
                "REJECT_LOAN_REQUEST",
                "OFFER_BACK")]
        public string Action { get; set; }
    }
}
