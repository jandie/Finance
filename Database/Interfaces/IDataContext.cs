﻿using Library.Classes;

namespace Database.Interfaces
{
    public interface IDataContext
    {
        User CreateUser(string name, string lastName, string email, string password, int currencyId, int languageId);
        User LoginUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions);
    }
}
