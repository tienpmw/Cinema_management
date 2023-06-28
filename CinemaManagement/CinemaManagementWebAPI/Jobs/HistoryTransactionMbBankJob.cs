using CinemaWebAPI.Banks;
using Quartz;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CinemaWebAPI.Utilities;
using DataAccess.DAOs;
using DTOs;

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
            return;
            MbBankRequestBodyHistoryTransactionDTO mbBank = new MbBankRequestBodyHistoryTransactionDTO()
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
            var request = await client.PostAsync(MbBankParameters.APIHistoryTransaction, content);

            var respone = await request.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonSerializerDateTimeConverter());
            options.Converters.Add(new JsonSerializerIntConverter());

            var data = JsonSerializer.Deserialize<MbBankResponeHistoryTransactionDataDTO>(respone, options);
            var transactionHistoryCreditList = data.transactionHistoryList.Where(x => x.creditAmount != 0).ToList();


            if (data.result.responseCode == "00")// get data success
            {
                string dataPreviousText = Util.Instance.ReadFile("Data/historyTransactionMbBank.json");
                List<TransactionHistory>? dataPrevious = JsonSerializer.Deserialize<List<TransactionHistory>>(dataPreviousText);

                // checking is first time run project
                if (true)
                {

                }

                // compare previous data with current data
                bool isSame = dataPrevious.SequenceEqual(transactionHistoryCreditList, new MbBankResponeHistoryTransactionDataEqualityComparer());
                if (isSame) return;

                //have new recharge info

                // save new data into file
                Util.Instance.WriteFile("Data/historyTransactionMbBank.json", transactionHistoryCreditList);
                //update DB
                RechargeRequestDAO.Instance.CheckingRecharge(transactionHistoryCreditList);
            }

        }
    }
}
