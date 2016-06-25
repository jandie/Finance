﻿using System;
using System.Text.RegularExpressions;
using Library.Interfaces;

namespace Library.Classes
{
    public class MonthlyBill : Payment, IPayment
    {
        public bool MayAddPayment => true;

        public MonthlyBill(int id, string name, decimal amount) : base(id, name, amount)
        {
            
        }

        public decimal GetSum()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            paid -= Amount;

            if (paid > 0) paid *= -1;

            return paid;
        }

        public decimal GetTotalAmount()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            return paid;
        }
    }
}
