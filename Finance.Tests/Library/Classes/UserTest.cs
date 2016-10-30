using System;
using Library.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

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
                _currency, Token);
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
            
        }

    }
}
