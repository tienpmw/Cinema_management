﻿using BusinessObject;
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

        public void Create(Transaction rechargeRequest) 
        {
            CinemaContext cinemaContext = new CinemaContext();  
            rechargeRequest.RequestDate = DateTime.Now;
            rechargeRequest.PaidDate = null;
            rechargeRequest.IsPay = false;
            rechargeRequest.UserId = 1;
            cinemaContext.Transaction.Add(rechargeRequest);
            cinemaContext.SaveChanges();
        }

        public void CheckingRecharge(List<TransactionHistory> transactionHistoryCreditList)
        {
            CinemaContext cinemaContext = new CinemaContext();
            var transaction = cinemaContext.Database.BeginTransaction();
            try
            {
                foreach (var item in transactionHistoryCreditList)
                {
                    if (IsRechargeRequestExisted(item.description, item.creditAmount))
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

        public bool IsRechargeRequestExisted(string desription, long amount)
        {
            CinemaContext cinemaContext = new CinemaContext();
            var recharge = cinemaContext.Transaction.FirstOrDefault(x => desription.Contains(x.Code) && x.Amount == amount);
            return recharge != null;
        }

    }
}
