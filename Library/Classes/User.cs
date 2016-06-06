using System.Collections.Generic;
using Library.Interfaces;

namespace Library.Classes
{
    public class User
    {
        private List<BankAccount> _bakBankAccounts;

        private List<IPayment> _payments;

        public List<BankAccount> BankAccounts => new List<BankAccount>(_bakBankAccounts);

        public List<IPayment> Payments => new List<IPayment>(_payments);

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;

            _bakBankAccounts = new List<BankAccount>();
            _payments = new List<IPayment>();
        }
    }
}
