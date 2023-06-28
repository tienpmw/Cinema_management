namespace CinemaWebAPI.Banks
{
    public class MbBankRequestBodyHistoryTransactionDTO
    {
        public string? accountNo { get; set; }
        public string? deviceIdCommon { get; set; }
        public string? fromDate { get; set; }
        public string? refNo { get; set; }
        public string? sessionId { get; set; }
        public string? toDate { get; set; }
    }
}
