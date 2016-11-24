using System;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        private InsertRepository()
        {
            _context = new InsertSqlContext();
        }

        public static InsertRepository Instance => _instance ?? (_instance = new InsertRepository());

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBankAccount(User user, string name, decimal balance, string password, string salt)
        {
            try
            {
                int id = _context.AddBankAccount(user.Id, name, balance, password, salt);

                Balance b = new Balance(id, name, balance);

                user.AddBalance(b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddPayment(User user, string name, decimal amount, PaymentType type, string password, string salt)
        {
            try
            {
                int id = _context.AddPayment(user.Id, name, amount, type, password, salt);

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddTransaction(IPayment payment, decimal amount, string description, string password, string salt)
        {
            try
            {
                int id = _context.AddTransaction(payment.Id, amount, description, password, salt);

                switch (payment.PaymentType)
                {
                    case PaymentType.MonthlyBill:
                        payment.AddTransaction(new Transaction(id, amount, description, false));

                        break;
                    case PaymentType.MonthlyIncome:
                        payment.AddTransaction(new Transaction(id, amount, description, true));

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}