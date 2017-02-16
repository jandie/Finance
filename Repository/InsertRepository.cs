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

        public InsertRepository()
        {
            _context = new InsertSqlContext();
        }

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
            finally
            {
                _context.CloseDb();
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
            finally
            {
                _context.CloseDb();
            }
        }

        public bool AddTransaction(User user, int paymentId, int balanceId, decimal amount, 
            string description, string password)
        {
            try
            {
                Transaction dummyTransaction;
                IPayment payment = user.GetPayment(paymentId);
                Balance balance = user.GetBalance(balanceId);

                if (payment == null) return false;

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

                dummyTransaction.Id = _context.AddTransaction(payment.Id, dummyTransaction, password);

                payment.AddTransaction(dummyTransaction);

                if (balance == null) return true;

                if (payment is MonthlyBill)
                {
                    new ChangeRepository().ChangeBalance(user, balance.Id, balance.Name, 
                        balance.BalanceAmount - amount, password);
                }
                else if (payment is MonthlyIncome)
                {
                    new ChangeRepository().ChangeBalance(user, balance.Id, balance.Name, 
                        balance.BalanceAmount + amount, password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                _context.CloseDb();
            }

            return true;
        }
    }
}