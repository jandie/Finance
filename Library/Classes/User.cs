﻿using System.Collections.Generic;
using Library.Interfaces;

namespace Library.Classes
{
    public class User
    {
        private List<Balance> _balances;

        private List<IPayment> _payments;

        public User(int id, string name, string lastName, string email, int languageId, Currency currency)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            LanguageId = languageId;
            Currency = currency;

            _balances = new List<Balance>();
            _payments = new List<IPayment>();
        }

        public List<Balance> Balances => new List<Balance>(_balances);

        public List<IPayment> Payments => new List<IPayment>(_payments);

        public int Id { get; }

        public string Name { get; }

        public string LastName { get; }

        public string Email { get; }

        public int LanguageId { get; }

        public Currency Currency { get; }

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

        public List<Transaction> TransactionList
        {
            get
            {
                List<Transaction> transactions = new List<Transaction>();

                _payments.ForEach(p => p.AllTransactions.ForEach(t => transactions.Add(new Transaction(t.Id, t.Amount, t.Description, t.Positive))));

                transactions.Sort();

                return transactions;
            }
        }

        public void AddBankAccounts(List<Balance> bankAccounts)
        {
            _balances = new List<Balance>(bankAccounts);
        }

        public void AddPayments(List<IPayment> payments)
        {
            _payments = new List<IPayment>(payments);
        }
    }
}
