namespace CinemaWebAPI.Banks
{
    public class MbBankParameters
    {
        public static string APIHistoryTransaction = "https://online.mbbank.com.vn/api/retail-web-transactionservice/transaction/getTransactionAccountHistory";
        public static string AccountNo = "0336687454";
        public static string DeviceIdCommon = "xp4czbl7-mbib-0000-0000-2023060214073587";
        public static string RefNo = "0336687454-2023062609580818";
        public static string SessionId = "ffb4da85-80b9-49bf-ae00-aa56aa5951e6";
        public static string FromDate = DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy");
        public static string ToDate = DateTime.Now.ToString("dd/MM/yyyy");

        public static string BasicAuthUserName = "EMBRETAILWEB";
        public static string BasicAuthPassword = "SD234dfg34%#@FG@34sfsdf45843f";
        public static string BasicAuthBase64 = "RU1CUkVUQUlMV0VCOlNEMjM0ZGZnMzQlI0BGR0AzNHNmc2RmNDU4NDNm";
    }
}
