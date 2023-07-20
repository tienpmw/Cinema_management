using BusinessObject;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class TransactionDAO
    {
        //Using Singleton Pattern
        private static TransactionDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TransactionDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TransactionDAO();
                    }
                }
                return instance;
            }
        }

        public void Create(Transaction transaction) 
        {
            CinemaContext cinemaContext = new CinemaContext();  
            transaction.RequestDate = DateTime.Now;
            transaction.PaidDate = null;
            transaction.IsPay = false;
            transaction.UserId = transaction.UserId;
            cinemaContext.Transaction.Add(transaction);
            cinemaContext.SaveChanges();
        }

        public void CheckingCreditTransactions(List<TransactionHistory> transactionHistoryCreditList)
        {
            CinemaContext cinemaContext = new CinemaContext();
            var transaction = cinemaContext.Database.BeginTransaction();
            try
            {
                foreach (var item in transactionHistoryCreditList)
                {
                    if (IsTransactionExisted(item.description, item.creditAmount))
                    {
                        var recharge = cinemaContext.Transaction.First(x => item.description.Contains(x.Code) && x.Amount == item.creditAmount);
                        if (recharge.IsPay == true) continue;
                        //update sataus recharge
                        recharge.IsPay = true;
                        recharge.PaidDate = System.DateTime.Now;
                        //update accout balace user
                        var user = cinemaContext.User.First(x => x.UserId == recharge.UserId);
                        user.AccountBalance = user.AccountBalance + item.creditAmount;  
                    }
                }
                cinemaContext.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex) 
            {
                transaction.Rollback();
            }
        }

        public void RemoveExpiredTransaction(int days)
        {
            CinemaContext cinemaContext = new CinemaContext();
            var transactionsExpired = cinemaContext.Transaction.Where(x => x.RequestDate < DateTime.Now.AddDays(-days) && x.IsPay == false).ToList();
            if (transactionsExpired.Count() == 0) return;
            cinemaContext.Transaction.RemoveRange(transactionsExpired);
            cinemaContext.SaveChanges();
        }

        public bool IsTransactionExisted(string desription, long amount)
        {
            CinemaContext cinemaContext = new CinemaContext();
            var transaction = cinemaContext.Transaction.FirstOrDefault(x => desription.Contains(x.Code) && x.Amount == amount);
            return transaction != null;
        }

    }
}
