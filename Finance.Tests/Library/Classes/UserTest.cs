using System;
using Library.Classes;
using Library.Enums;
using Library.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finance.Tests.Library.Classes
{
    [TestClass]
    public class UserTest
    {
        private const int Id = 0;
        private const string Name = "jandie";
        private const string LastName = "hendriks";
        private const string Email = "jandie@hendriks.com";
        private const int LanguageId = 0;
        private const string Token = "hjwbdqui32uhewyr3982iowjq9ioheusygvhwadsb";
        private const string Salt = "x31717c1x11xn78yn178y178r==";
        private User _user;
        private const int CurrencyId = 0;
        private const string CurrencyAbbrevation = "eur";
        private const string CurrencyName = "euro";
        private const string CurrencyHtml = "eurohtml";
        private Currency _currency;

        [TestInitialize]
        public void Init()
        {
            _currency = new Currency(CurrencyId, CurrencyAbbrevation, 
                CurrencyName, CurrencyHtml);
            _user = new User(Id, Name, LastName, Email, LanguageId,
                _currency, Token, Salt);
        }

        [TestMethod]
        public void TestGetId()
        {
            Assert.AreEqual(Id, _user.Id);
        }

        [TestMethod]
        public void TestGetName()
        {
            Assert.AreEqual(Name, _user.Name);
        }

        [TestMethod]
        public void TestGetLastName()
        {
            Assert.AreEqual(LastName, _user.LastName);
        }

        [TestMethod]
        public void TestGetEmail()
        {
            Assert.AreEqual(Email, _user.Email);
        }

        [TestMethod]
        public void TestGetLanguageId()
        {
            Assert.AreEqual(LanguageId, _user.LanguageId);
        }

        [TestMethod]
        public void TestGetCurrency()
        {
            Assert.AreEqual(_currency, _user.Currency);
        }

        [TestMethod]
        public void TestGetToken()
        {
            Assert.AreEqual(Token, _user.Token);
        }

        [TestMethod]
        public void TestGetTotalBalance()
        {
            Balance balance1 = new Balance(0, "", 10);
            Balance balance2 = new Balance(1, "", -10);
            Balance balance3 = new Balance(2, "", 0);
            Balance balance4 = new Balance(3, "", (decimal)10.22);

            _user.AddBalance(balance1);
            _user.AddBalance(balance2);
            _user.AddBalance(balance3);
            _user.AddBalance(balance4);

            Assert.AreEqual(4, _user.Balances.Count);

            decimal expectedResult = balance1.BalanceAmount +
                balance2.BalanceAmount +
                balance3.BalanceAmount +
                balance4.BalanceAmount;

            Assert.AreEqual(expectedResult, _user.TotalBalance);

            _user.DeleteBalance(0);
            _user.DeleteBalance(1);
            _user.DeleteBalance(2);
            _user.DeleteBalance(3);

            Assert.AreEqual(0, _user.Balances.Count);
        }

        [TestMethod]
        public void TestGetPrediction()
        {
            //Create new balances.
            Balance balance1 = new Balance(0, "", 10);
            Balance balance2 = new Balance(1, "", -10);
            Balance balance3 = new Balance(2, "", 0);
            Balance balance4 = new Balance(3, "", (decimal)10.22);

            //Add the new balances.
            _user.AddBalance(balance1);
            _user.AddBalance(balance2);
            _user.AddBalance(balance3);
            _user.AddBalance(balance4);

            Assert.AreEqual(4, _user.Balances.Count);

            //Create new payments.
            IPayment bill1 = 
                new MonthlyBill(0, "daily", 500, PaymentType.MonthlyBill);
            IPayment bill2 =
                new MonthlyBill(1, "extra", 1, PaymentType.MonthlyBill);
            IPayment income1 = 
                new MonthlyIncome(2, "salary", 700, PaymentType.MonthlyIncome);
            IPayment income2 =
                new MonthlyIncome(3, "extra", 50, PaymentType.MonthlyIncome);

            //Create new transactions.
            Transaction tBill1 = new Transaction(0, 5, "food", false);
            Transaction tBill2 = new Transaction(1, (decimal)1.0, "payback", false);
            Transaction tInc1 = new Transaction(2, 700, "work", true);
            Transaction tInc2 = new Transaction(3, 5, "money found on street", true);

            //Add transactions to the payments.
            bill1.AddTransaction(tBill1);
            bill2.AddTransaction(tBill2);
            income1.AddTransaction(tInc1);
            income2.AddTransaction(tInc2);

            Assert.AreEqual(1, bill1.AllTransactions.Count);
            Assert.AreEqual(1, bill2.AllTransactions.Count);
            Assert.AreEqual(1, income1.AllTransactions.Count);
            Assert.AreEqual(1, income2.AllTransactions.Count);

            //Add payments to user.
            _user.AddPayment(bill1);
            _user.AddPayment(bill2);
            _user.AddPayment(income1);
            _user.AddPayment(income2);

            Assert.AreEqual(4, _user.Payments.Count);

            //Sum of all balances.
            decimal totalOfBalances = balance1.BalanceAmount +
                balance2.BalanceAmount +
                balance3.BalanceAmount +
                balance4.BalanceAmount;

            //Sum of all payments and their transactions.
            decimal totalOfPayments = -bill1.Amount + tBill1.Amount + 
                -bill2.Amount + tBill2.Amount +
                income1.Amount + -tInc1.Amount +
                income2.Amount + -tInc2.Amount;

            //The expected prediction.
            decimal expectedPrediction = totalOfBalances + totalOfPayments;

            Assert.AreEqual(expectedPrediction, _user.Prediction);

            //Delete all payments added.
            _user.DeletePayment(0);
            _user.DeletePayment(1);
            _user.DeletePayment(2);
            _user.DeletePayment(3);

            Assert.AreEqual(0, _user.Payments.Count);

            //Delete all balances added.
            _user.DeleteBalance(0);
            _user.DeleteBalance(1);
            _user.DeleteBalance(2);
            _user.DeleteBalance(3);

            Assert.AreEqual(0, _user.Balances.Count);
        }

        [TestMethod]
        public void TestGetToPay()
        {
            //Create new payments.
            IPayment bill1 =
                new MonthlyBill(0, "daily", 500, PaymentType.MonthlyBill);
            IPayment bill2 =
                new MonthlyBill(1, "extra", 1, PaymentType.MonthlyBill);

            //Create new transactions.
            Transaction tBill1 = new Transaction(0, 5, "food", false);
            Transaction tBill2 = new Transaction(1, (decimal)1.0, "payback", false);

            //Add transactions to the payments.
            bill1.AddTransaction(tBill1);
            bill2.AddTransaction(tBill2);

            Assert.AreEqual(1, bill1.AllTransactions.Count);
            Assert.AreEqual(1, bill2.AllTransactions.Count);

            //Add payments to user.
            _user.AddPayment(bill1);
            _user.AddPayment(bill2);

            Assert.AreEqual(2, _user.Payments.Count);

            //Sum of all payments and their transactions.
            decimal totalOfBills = Math.Abs(-bill1.Amount + tBill1.Amount +
                                      -bill2.Amount + tBill2.Amount);

            Assert.AreEqual(totalOfBills, _user.ToPay);

            //Delete all payments added.
            _user.DeletePayment(0);
            _user.DeletePayment(1);

            Assert.AreEqual(0, _user.Payments.Count);
        }

        [TestMethod]
        public void TestGetToGet()
        {
            //Create new payments.
            IPayment income1 =
                new MonthlyIncome(2, "salary", 700, PaymentType.MonthlyIncome);
            IPayment income2 =
                new MonthlyIncome(3, "extra", 50, PaymentType.MonthlyIncome);

            //Create new transactions.
            Transaction tInc1 = new Transaction(2, 700, "work", true);
            Transaction tInc2 = new Transaction(3, 5, "money found on street", true);

            //Add transactions to the payments.
            income1.AddTransaction(tInc1);
            income2.AddTransaction(tInc2);

            Assert.AreEqual(1, income1.AllTransactions.Count);
            Assert.AreEqual(1, income2.AllTransactions.Count);

            //Add payments to user.
            _user.AddPayment(income1);
            _user.AddPayment(income2);

            Assert.AreEqual(2, _user.Payments.Count);

            //Sum of all payments and their transactions.
            decimal totalOfIncome = income1.Amount + -tInc1.Amount +
                income2.Amount + -tInc2.Amount;

            Assert.AreEqual(totalOfIncome, _user.ToGet);

            //Delete all payments added.
            _user.DeletePayment(2);
            _user.DeletePayment(3);

            Assert.AreEqual(0, _user.Payments.Count);
        }

        [TestMethod]
        public void TestGetTransactionList()
        {
            //Create new payments.
            IPayment bill1 =
                new MonthlyBill(0, "daily", 500, PaymentType.MonthlyBill);
            IPayment bill2 =
                new MonthlyBill(1, "extra", 1, PaymentType.MonthlyBill);
            IPayment income1 =
                new MonthlyIncome(2, "salary", 700, PaymentType.MonthlyIncome);
            IPayment income2 =
                new MonthlyIncome(3, "extra", 50, PaymentType.MonthlyIncome);

            //Create new transactions.
            Transaction tBill1 = new Transaction(0, 5, "food", false);
            Transaction tBill2 = new Transaction(1, (decimal)1.0, "payback", false);
            Transaction tInc1 = new Transaction(2, 700, "work", true);
            Transaction tInc2 = new Transaction(3, 5, "money found on street", true);

            //Add transactions to the payments.
            bill1.AddTransaction(tBill1);
            bill2.AddTransaction(tBill2);
            income1.AddTransaction(tInc1);
            income2.AddTransaction(tInc2);

            Assert.AreEqual(1, bill1.AllTransactions.Count);
            Assert.AreEqual(1, bill2.AllTransactions.Count);
            Assert.AreEqual(1, income1.AllTransactions.Count);
            Assert.AreEqual(1, income2.AllTransactions.Count);

            //Add payments to user.
            _user.AddPayment(bill1);
            _user.AddPayment(bill2);
            _user.AddPayment(income1);
            _user.AddPayment(income2);

            Assert.AreEqual(4, _user.Payments.Count);

            Assert.AreEqual(4, _user.TransactionList.Count);

            //Delete all payments added.
            _user.DeletePayment(0);
            _user.DeletePayment(1);
            _user.DeletePayment(2);
            _user.DeletePayment(3);

            Assert.AreEqual(0, _user.Payments.Count);
        }

    }
}
