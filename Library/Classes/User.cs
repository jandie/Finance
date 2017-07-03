using System;
using System.Collections.Generic;
using System.Linq;
using Library.Interfaces;

namespace Library.Classes
{
    public class User
    {
        private readonly List<Balance> _balances;

        private readonly List<IPayment> _payments;

        private readonly List<BalanceHistory> _balanceHistories;

        public User(int id, string name, string lastName, string email, int languageId, Currency currency, string token, string salt)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            LanguageId = languageId;
            Currency = currency;
            Token = token;
            Salt = salt;

            _balances = new List<Balance>();
            _payments = new List<IPayment>();
            _balanceHistories =  new List<BalanceHistory>();
        }

        public List<Balance> Balances => new List<Balance>(_balances);

        public List<IPayment> Payments => new List<IPayment>(_payments);

        public int Id { get; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; }

        public int LanguageId { get; set; }

        public Currency Currency { get; }

        public string Token { get; }

        public string Salt { get; }

        public decimal TotalBalance
        {
            get
            {
                decimal total = 0;

                Balances.ForEach(b => total += b.BalanceAmount);

                return total;
            }
        }

        public decimal Prediction
        {
            get
            {
                decimal totalBalance = 0;
                decimal sum = 0;
                decimal prediction = 0;

                Balances.ForEach(b => totalBalance += b.BalanceAmount);

                Payments.ForEach(p => sum += p.GetSum());

                prediction = totalBalance + sum;

                return prediction;
            }
        }

        public decimal ToPay
        {
            get
            {
                decimal toPay = 0;

                Payments.OfType<MonthlyBill>().ToList().ForEach(p => toPay += Math.Abs(p.GetSum()));

                return toPay;
            }
        }

        public decimal ToGet
        {
            get
            {
                decimal toGet = 0;

                Payments.OfType<MonthlyIncome>().ToList().ForEach(p => toGet += p.GetSum());

                return toGet;
            }
        }

        public List<Transaction> TransactionList
        {
            get
            {
                List<Transaction> transactions = new List<Transaction>();

                _payments.ForEach(
                    p =>
                        p.AllTransactions.ForEach(
                            t => transactions
                            .Add(new Transaction(t.Id, t.Amount, t.Description, 
                                t.Positive))));

                transactions.Sort();

                return transactions;
            }
        }

        public Transaction GetTransaction(int transactionId)
        {
            return Payments
                .Find(p => p.AllTransactions.Any(t => t.Id == transactionId))
                .AllTransactions.Find(t => t.Id == transactionId);
        }

        public void AddBalance(Balance bankAccount)
        {
            _balances.Add(bankAccount);
        }

        public Balance GetBalance(int balanceId)
        {
            return Balances.Find(b => b.Id == balanceId);
        }

        public void DeleteBalance(int id)
        {
            _balances.Remove(_balances.Find(b => b.Id == id));
        }

        public void AddPayment(IPayment payment)
        {
            _payments.Add(payment);
        }

        public void DeletePayment(int id)
        {
            _payments.Remove(_payments.Find(p => p.Id == id));
        }

        public IPayment GetPayment(int paymentId)
        {
            return Payments.Find(p => p.Id == paymentId);
        }

        public IPayment GetPaymentByTransaction(int transactionId)
        {
            return Payments
                .Find(p => p.AllTransactions.Any(t => t.Id == transactionId));
        }

        public void AddBalanceHistory(BalanceHistory balanceHistory)
        {
            _balanceHistories.Add(balanceHistory);
        }

        public void DeleteBalanceHistory(int id)
        {
            BalanceHistory balanceHistoryToRemove = _balanceHistories.Find(b => b.Id == id);

            if (balanceHistoryToRemove != null) _balanceHistories.Remove(balanceHistoryToRemove);
        }
    }
}