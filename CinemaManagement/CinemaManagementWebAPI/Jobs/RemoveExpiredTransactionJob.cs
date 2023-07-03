using DataAccess.DAOs;
using Quartz;

namespace CinemaWebAPI.Jobs
{
    public class RemoveExpiredTransactionJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            TransactionDAO.Instance.RemoveExpiredTransaction(3);
        }
    }
}
