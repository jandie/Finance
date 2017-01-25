using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Enums;
using Library.Interfaces;

namespace Repository
{
    public class InsertRepository
    {
        private readonly IInsertContext _context;

        private InsertRepository()
        {
            _context = new InsertSqlContext();
        }

        public static InsertRepository Instance => new InsertRepository();

        public void AddBankAccount(User user, string name, decimal balance, string password)
        {
            try
            {
                Balance dummyBalance = new Balance(-1, name, balance);

                int id = _context.AddBankAccount(user.Id, dummyBalance, password);

                dummyBalance.Id = id;

                user.AddBalance(dummyBalance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddPayment(User user, string name, decimal amount, PaymentType type, string password)
        {
            try
            {
                Payment dummyPayment;

                switch (type)
                {
                    case PaymentType.MonthlyBill:
                        dummyPayment = new MonthlyBill(-1, name, amount, type);

                        break;
                    case PaymentType.MonthlyIncome:
                        dummyPayment = new MonthlyIncome(-1, name, amount, type);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                int id = _context.AddPayment(user.Id, dummyPayment, password);

                dummyPayment.Id = id;

                user.AddPayment((IPayment) dummyPayment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddTransaction(IPayment payment, decimal amount, string description, string password)
        {
            try
            {
                Transaction dummyTransaction;

                switch (payment.PaymentType)
                {
                    case PaymentType.MonthlyBill:
                        dummyTransaction = new Transaction(-1, amount, description, false);

                        break;
                    case PaymentType.MonthlyIncome:
                        dummyTransaction = new Transaction(-1, amount, description, true);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                int id = _context.AddTransaction(payment.Id, dummyTransaction, password);

                dummyTransaction.Id = id;

                payment.AddTransaction(dummyTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}