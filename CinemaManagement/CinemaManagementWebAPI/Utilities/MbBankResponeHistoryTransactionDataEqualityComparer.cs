﻿using CinemaWebAPI.Banks;
using DTOs;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CinemaWebAPI.Utilities
{
    public class MbBankResponeHistoryTransactionDataEqualityComparer : IEqualityComparer<TransactionHistory>
    {
        public bool Equals(TransactionHistory? x, TransactionHistory? y)
        {
            if(x.postingDate == y.postingDate &&
               x.transactionDate == y.transactionDate &&
               x.accountNo == y.accountNo &&
               x.creditAmount == y.creditAmount &&
               x.refNo == y.refNo)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode([DisallowNull] TransactionHistory obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + obj.refNo.GetHashCode();
                hash = hash * 23 + obj.transactionDate.GetHashCode();
                hash = hash * 23 + obj.creditAmount.GetHashCode();
                return hash;
            }
        }
    }
}
