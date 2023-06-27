using BusinessObject;
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

        
    }
}
