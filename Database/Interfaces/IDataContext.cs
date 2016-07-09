using System.Collections.Generic;
using Library.Classes;
using Library.Classes.Language;

namespace Database.Interfaces
{
    public interface IDataContext
    {
        User CreateUser(string name, string lastName, string email, string password, int currencyId, int languageId);

        User LoginUser(string email, string password);

        User LoadUser(string email);

        List<Currency> LoadCurrencies();

        List<Language> LoadLanguages();
    }
}