using BusinessObject;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class RechargeRequestDAO
    {
        //Using Singleton Pattern
        private static RechargeRequestDAO instance = null;
        private static readonly object instanceLock = new object();

        public static RechargeRequestDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RechargeRequestDAO();
                    }
                }
                return instance;
            }
        }

        public void Create(RechargeRequest rechargeRequest) 
        {
            rechargeRequest.RequestDate = DateTime.Now;
            rechargeRequest.PaidDate = null;
            rechargeRequest.IsPay = false;
            CinemaContext.Instance.RechargeRequest.Add(rechargeRequest);
            CinemaContext.Instance.SaveChanges();
        }

        public void CheckingRecharge(List<TransactionHistory> transactionHistoryCreditList)
        {
            var transaction = CinemaContext.Instance.Database.BeginTransaction();
            try
            {
                foreach (var item in transactionHistoryCreditList)
                {
                    if (IsRechargeRequestExisted(item.description, item.creditAmount))
                    {
                        var recharge = CinemaContext.Instance.RechargeRequest.First(x => x.Code == item.description && x.Amount == item.creditAmount);
                        if (recharge.IsPay == true) continue;
                        //update sataus recharge
                        recharge.IsPay = true;
                        //update accout balace user
                        var user = CinemaContext.Instance.User.First(x => x.UserId == recharge.UserId);
                        user.AccountBalance = user.AccountBalance + item.creditAmount;  
                    }
                }
                CinemaContext.Instance.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex) 
            {
                transaction.Rollback();
            }
        }

        public bool IsRechargeRequestExisted(string desription, long amount)
        {
            var recharge = CinemaContext.Instance.RechargeRequest.FirstOrDefault(x => x.Code == desription && x.Amount == amount);
            return recharge != null;
        }

        public RechargeRequest GetRechargeRequest(string desription, long amount)
        {
            var recharge = CinemaContext.Instance.RechargeRequest.FirstOrDefault(x => x.Code == desription && x.Amount == amount);
            return recharge;
        }
    }
}
