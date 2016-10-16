using System;
using System.Collections.Generic;
using System.Globalization;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Enums;
using Library.Interfaces;

namespace Repository
{
    public class InsertRepository
    {
        private static InsertRepository _instance;
        private readonly IInsertContext _context;

        private InsertRepository()
        {
            _context = new InsertSqlContext();
        }

        public static InsertRepository Instance => _instance ?? (_instance = new InsertRepository());

        public void AddBankAccount(User user, string name, decimal balance)
        {
            try
            {
                int id = _context.AddBankAccount(user.Id, name, balance);

                Balance b = new Balance(id, name, balance);

                user.AddBankAccount(b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddPayment(User user, string name, decimal amount, PaymentType type)
        {
            try
            {
                int id = _context.AddPayment(user.Id, name, amount, type);

                IPayment payment;

                switch (type)
                {
                    case PaymentType.MonthlyBill:
                        payment = new MonthlyBill(id, name, amount, type);

                        break;
                    case PaymentType.MonthlyIncome:
                        payment = new MonthlyIncome(id, name, amount, type);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                user.AddPayment(payment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddTransaction(IPayment payment, decimal amount, string description)
        {
            try
            {
                int id = _context.AddTransaction(payment.Id, amount, description);

                payment.AddTransaction(new Transaction(id, amount, description, amount >= 0));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}