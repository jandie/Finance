using System.Collections.Generic;
using FinanceLibrary.Classes;
using FinanceLibrary.Classes.Language;

namespace FinanceLibrary.Repository.Interfaces
{
    public interface IDataContext
    {
        /// <summary>
        /// Creates a new user and returns the user.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="currencyId">The id of the preferred currency.</param>
        /// <param name="languageId">The id of the preferred language.</param>
        /// <returns>A user that has been loaded from the database.</returns>
        User CreateUser(string name, string lastName, string email, string password, int currencyId, int languageId);

        /// <summary>
        /// Loads the user and checks whether or not the password is correct.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A user that has been loaded from the database.</returns>
        User LoginUser(string email, string password);

        /// <summary>
        /// Checks wether or not the token of the user was changed.
        /// </summary>
        /// <param name="email">The email of the user where the token is checked.</param>
        /// <param name="token">The token itself.</param>
        /// <returns>Wether or not the token is changed.</returns>
        bool TokenChanged(string email, string token);

        /// <summary>
        /// Loads all exsisting currencies from the database.
        /// </summary>
        /// <returns>A list of all exsisting currencies.</returns>
        List<Currency> LoadCurrencies();

        /// <summary>
        /// Loads all exsisting languages from the database.
        /// </summary>
        /// <returns>A list of all exsisting languages in the database.</returns>
        List<Language> LoadLanguages();

        /// <summary>
        /// Loads all balances of the user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <returns>List of balances of the user.</returns>
        List<Balance> GetBalancesOfUser(int userId, string password);

        /// <summary>
        /// Loads payments of a user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <returns>List of payments of the user.</returns>
        List<IPayment> GetPaymentsOfUser(int userId, string password);

        /// <summary>
        /// Loads the transactions of a payment from the database.
        /// </summary>
        /// <param name="payment">The payment itself.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <param name="monthYear">Year and month of the transactions to load. 
        /// Leave empty to load all transactions.
        /// Example: 2015-01</param>
        /// <returns>List of transactions of the payment.</returns>
        List<Transaction> GetTransactionsOfPayment(IPayment payment, string password,
            string monthYear = null);
    }
}