using CinemaWebAPI.Banks;
using Quartz;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CinemaWebAPI.Utilities;

namespace CinemaWebAPI.Jobs
{
    public class HistoryTransactionMbBankJob : IJob
    {
        private readonly HttpClient client = null;

        public HistoryTransactionMbBankJob()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + MbBankParameters.BasicAuthBase64);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            MbBankRequestBodyHistoryTransaction mbBank = new MbBankRequestBodyHistoryTransaction()
            {
                accountNo = MbBankParameters.AccountNo,
                deviceIdCommon = MbBankParameters.DeviceIdCommon,
                refNo = MbBankParameters.RefNo,
                fromDate = MbBankParameters.FromDate,
                sessionId = MbBankParameters.SessionId,
                toDate = MbBankParameters.ToDate
            };

            var jsonData = JsonSerializer.Serialize(mbBank);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var request = await client.PostAsync(MbBankParameters.API, content);

            var respone = await request.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonSerializerDateTimeConverter());
            options.Converters.Add(new JsonSerializerIntConverter());
            var data = JsonSerializer.Deserialize<MbBankResponeHistoryTransactionData>(respone, options);

        }
    }
}
