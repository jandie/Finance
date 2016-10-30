using System;
using Library.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finance.Tests.Library.Classes
{
    [TestClass]
    public class UserTest
    {
        private User _user;

        [TestInitialize]
        public void Init()
        {
            Currency currency = new Currency(0, "eur", "euro", "eurohtml");
            _user = new User(99, "jandie", "hendriks", "jandie@hendriks.com", 0, currency, "d78237e287ncey2n3ey8n23ex23ney");
        }

        [TestMethod]
        public void TestGetName()
        {
            Assert.AreEqual("jandie", _user.Name);
        }
    }
}
