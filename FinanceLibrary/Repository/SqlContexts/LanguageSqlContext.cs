using System;
using System.Collections.Generic;
using System.Data;
using FinanceLibrary.Classes.Language;
using FinanceLibrary.Repository.Interfaces;

namespace FinanceLibrary.Repository.SqlContexts
{
    public class LanguageSqlContext : ILanguageContext
    {
        private readonly Database _db;

        public LanguageSqlContext()
        {
            _db = new Database();
        }
        /// <summary>
        /// Deletes all rows from the language tables.
        /// </summary>
        public void Clean()
        {
            CleanTranslations();
            CleanLanguages();
        }

        /// <summary>
        /// Adds a language to the database.
        /// </summary>
        /// <param name="language">The language to add.</param>
        public void AddLaguage(Language language)
        {
            const string query = "INSERT INTO LANGUAGE (ID, ABBREVATION, NAME) VALUES (@Id, @Abbrevation, @Name)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Id", language.Id},
                {"Abbrevation", language.Abbrevation},
                {"Name", language.Name}
            };

            _db.Execute(query, parameters, Database.QueryType.NonQuery);

            language.Translations.ForEach(t => AddTranslation(t, language.Id));
        }

        /// <summary>
        /// Loads a language from the database.
        /// </summary>
        /// <param name="languageId">The id of the language to identify.</param>
        /// <returns>A language</returns>
        public Language LoadLanguage(int languageId)
        {
            Language language;
            const string query = "SELECT ID, ABBREVATION, NAME FROM LANGUAGE WHERE ID = @languageId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"languageId", languageId}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return null;
                DataRow row = table.Rows[0];

                int id = Convert.ToInt32(row[0]);
                string abbrevation = row[1] as string;
                string name = row[2] as string;

                language = new Language(id, abbrevation, name);
            }

            LoadTranslations(languageId).ForEach(t => language?.AddTranslation(t));

            return language;
        }

        /// <summary>
        /// Deletes all rows from translation table.
        /// </summary>
        private void CleanTranslations()
        {
            _db.Execute("DELETE FROM TRANSLATION", null, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Deletes all rows from language table.
        /// </summary>
        private void CleanLanguages()
        {
            _db.Execute("DELETE FROM LANGUAGE", null, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Adds a translation to the database.
        /// </summary>
        /// <param name="translation">The translation to add.</param>
        /// <param name="languageId">The id of the language the translation belongs to.</param>
        private void AddTranslation(Translation translation, int languageId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Id", translation.Id},
                {"languageId", languageId},
                {"TranslationText", translation.TranslationText}
            };

            _db.Execute("INSERT INTO TRANSLATION (ID, LANGUAGE_ID, TRANSLATION) VALUES (@Id, @languageId, @TranslationText)", 
                parameters, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Loads translations of a language from the database.
        /// </summary>
        /// <param name="languageId">The id of the language the translations belong to.</param>
        /// <returns>List of translations</returns>
        private List<Translation> LoadTranslations(int languageId)
        {
            List<Translation> translations = new List<Translation>();
            const string query = "SELECT ID, TRANSLATION FROM TRANSLATION WHERE LANGUAGE_ID = @languageId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"languageId", languageId}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return translations;
                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string translationText = row[1] as string;

                    translations.Add(new Translation(id, translationText));
                }
            }

            return translations;
        }
    }
}