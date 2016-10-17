using System.Collections.Generic;
using Library.Classes;
using Library.Classes.Language;

namespace Database.Interfaces
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

        bool TokenChanged(string email, string token);

        /// <summary>
        /// Loads a user from the database.
        /// </summary>
        /// <param name="email">The email of the user (to identify).</param>
        /// <returns>A user that has been loaded from the database.</returns>
        User LoadUser(string email);

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
    }
}