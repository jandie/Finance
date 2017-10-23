using System;
using FinanceLibrary.Classes;
using FinanceLibrary.Enums;
using FinanceLibrary.Repository;
using FinanceLibrary.Repository.Interfaces;
using FinanceLibrary.Repository.SqlContexts;

namespace FinanceLibrary.Logic
{
    public class InsertLogic
    {
        private readonly IInsertContext _context;
        private readonly Database _database;

        public InsertLogic()
        {
            _database =  new Database();
            _context = new InsertSqlContext(_database);
        }

        /// <summary>
        /// Adds balance to a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the balance.</param>
        /// <param name="balance">The balance amount of the balance.</param>
        public void AddBankAccount(User user, string name, decimal balance)
        {
            try
            {
                Balance dummyBalance = new Balance(-1, name, balance);

                int id = _context.AddBankAccount(user.Id, dummyBalance, user.MasterPassword);

                dummyBalance.Id = id;

                user.AddBalance(dummyBalance);

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Adds a payment to a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <param name="type">The type of payment.</param>
        public void AddPayment(User user, string name, decimal amount, PaymentType type)
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

                int id = _context.AddPayment(user.Id, dummyPayment, user.MasterPassword);

                dummyPayment.Id = id;

                user.AddPayment((IPayment) dummyPayment);

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Adds a transaction to a balance of a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="paymentId">The id of the transaction.</param>
        /// <param name="balanceId">The id of the balance the transaction belongs to.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <returns>Whether or not the action was a success.</returns>
        public bool AddTransaction(User user, int paymentId, int balanceId, decimal amount, 
            string description)
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

                dummyTransaction.Id = _context.AddTransaction(payment.Id, dummyTransaction, user.MasterPassword);

                payment.AddTransaction(dummyTransaction);

                if (balance == null) return true;

                switch (payment)
                {
                    case MonthlyBill _:
                        new ChangeLogic().ChangeBalance(user, balance.Id, balance.Name, 
                            balance.BalanceAmount - amount);
                        break;
                    case MonthlyIncome _:
                        new ChangeLogic().ChangeBalance(user, balance.Id, balance.Name, 
                            balance.BalanceAmount + amount);
                        break;
                }

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }
    }
}