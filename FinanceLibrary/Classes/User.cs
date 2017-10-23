using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceLibrary.Classes
{
    public class User
    {
        /// <summary>
        /// List containing Balance objects of the User.
        /// </summary>
        private readonly List<Balance> _balances;

        /// <summary>
        /// List containing Payment objects of the User.
        /// </summary>
        private readonly List<IPayment> _payments;

        /// <summary>
        /// List containing BalanceHistory objects of the user.
        /// </summary>
        private List<BalanceHistory> _balanceHistories;

        /// <summary>
        /// Creates an instance of the User object.
        /// </summary>
        /// <param name="id">The ID of the User.</param>
        /// <param name="name">The name of the User.</param>
        /// <param name="lastName">The lastname of the User.</param>
        /// <param name="email">The email of the User.</param>
        /// <param name="languageId">The languageID of the User.</param>
        /// <param name="currency">The currency preference of the User.</param>
        /// <param name="token">The token of the User. Used for checking User changes.</param>
        /// <param name="salt">The salt of the User. User for encryption.</param>
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

        /// <summary>
        /// Master password of the user, used for encryption.
        /// </summary>
        public string MasterPassword { get; set; }

        /// <summary>
        /// Salt of masterpassword used for encryption.
        /// </summary>
        public string MasterSalt { get; set; }

        /// <summary>
        /// Returns a copy of the List of Balance objects of the User.
        /// </summary>
        public List<Balance> Balances => new List<Balance>(_balances);

        /// <summary>
        /// Returns a copy of the List of Payment objects of the User.
        /// </summary>
        public List<IPayment> Payments => new List<IPayment>(_payments);

        /// <summary>
        /// The ID of the User.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of the User.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The lastname of the User.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the User.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// The ID of the Language of the User.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// The Currency preference of the User.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// The token of the User. User for detecting changes in User.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// The salt of the User. Used for encryption.
        /// </summary>
        public string Salt { get; }

        /// <summary>
        /// List of BalanceHistory objects of the User. Getter and Setter both use a copy of the object.
        /// </summary>
        public List<BalanceHistory> BalanceHistories {
            get => new List<BalanceHistory>(_balanceHistories);
            set => _balanceHistories = new List<BalanceHistory>(value);
        }

        /// <summary>
        /// Calculates the total balance.
        /// </summary>
        public decimal TotalBalance
        {
            get
            {
                decimal total = 0;

                Balances.ForEach(b => total += b.BalanceAmount);

                return total;
            }
        }

        /// <summary>
        /// Calculates the balance prediction for the end of the month.
        /// </summary>
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

        /// <summary>
        /// Calculates the amount yet to pay.
        /// </summary>
        public decimal ToPay
        {
            get
            {
                decimal toPay = 0;

                Payments.OfType<MonthlyBill>().ToList().ForEach(p => toPay += Math.Abs(p.GetSum()));

                return toPay;
            }
        }

        /// <summary>
        /// Calculates the amount yet to get.
        /// </summary>
        public decimal ToGet
        {
            get
            {
                decimal toGet = 0;

                Payments.OfType<MonthlyIncome>().ToList().ForEach(p => toGet += p.GetSum());

                return toGet;
            }
        }

        /// <summary>
        /// Gets a sorted transactionlist. All objects are new instances of the real objects.
        /// </summary>
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

        /// <summary>
        /// Gets a Transaction based on its ID.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>The ID of the Transaction.</returns>
        public Transaction GetTransaction(int transactionId)
        {
            return Payments
                .Find(p => p.AllTransactions.Any(t => t.Id == transactionId))
                .AllTransactions.Find(t => t.Id == transactionId);
        }

        /// <summary>
        /// Adds a new Balance to the User.
        /// </summary>
        /// <param name="bankAccount">The new Balance.</param>
        public void AddBalance(Balance bankAccount)
        {
            _balances.Add(bankAccount);
        }

        /// <summary>
        /// Gets Balance of the User based on the ID of the Balance.
        /// </summary>
        /// <param name="balanceId">The ID of the Balance.</param>
        /// <returns></returns>
        public Balance GetBalance(int balanceId)
        {
            return Balances.Find(b => b.Id == balanceId);
        }

        /// <summary>
        /// Deletes a Balance from the User based on the ID of the Balance.
        /// </summary>
        /// <param name="id">The ID of the Balance.</param>
        public void DeleteBalance(int id)
        {
            _balances.Remove(_balances.Find(b => b.Id == id));
        }

        /// <summary>
        /// Adds a new Payment to the User.
        /// </summary>
        /// <param name="payment">The new Payment.</param>
        public void AddPayment(IPayment payment)
        {
            _payments.Add(payment);
        }

        /// <summary>
        /// Deletes a Payment from the User based on the ID of the Payment.
        /// </summary>
        /// <param name="id">The ID of the Payment.</param>
        public void DeletePayment(int id)
        {
            _payments.Remove(_payments.Find(p => p.Id == id));
        }

        /// <summary>
        /// Gets a Payment based on it's ID.
        /// </summary>
        /// <param name="paymentId">The ID of the Payment.</param>
        /// <returns></returns>
        public IPayment GetPayment(int paymentId)
        {
            return Payments.Find(p => p.Id == paymentId);
        }

        /// <summary>
        /// Gets a Payment based on the ID of a Transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public IPayment GetPaymentByTransaction(int transactionId)
        {
            return Payments
                .Find(p => p.AllTransactions.Any(t => t.Id == transactionId));
        }

        /// <summary>
        /// Adds a BalanceHistory to the User.
        /// </summary>
        /// <param name="balanceHistory"></param>
        public void AddBalanceHistory(BalanceHistory balanceHistory)
        {
            DeleteBalanceHistory(balanceHistory.DateTime);

            _balanceHistories.Add(balanceHistory);
        }

        /// <summary>
        /// Deletes a BalanceHistory from the used base om the Date of the BalanceHistory.
        /// </summary>
        /// <param name="dateTime">The Date of the BalanceHistory.</param>
        public void DeleteBalanceHistory(DateTime dateTime)
        {
            BalanceHistory balanceHistoryToRemove = _balanceHistories.Find(b => b.DateTime.Date == dateTime.Date);

            if (balanceHistoryToRemove != null) _balanceHistories.Remove(balanceHistoryToRemove);
        }
    }
}