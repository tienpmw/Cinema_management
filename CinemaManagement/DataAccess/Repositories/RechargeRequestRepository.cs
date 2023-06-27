﻿using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RechargeRequestRepository : IRechargeRequestRepository
    {
        public void CreateRechargeRequest(RechargeRequest rechargeRequest)
        {
            RechargeRequestDAO.Instance.Create(rechargeRequest);
        }
    }
}
