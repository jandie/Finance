using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes.Language;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class LanguageSqlContext : ILanguageContext
    {
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
            MySqlConnection connection = Database.NewInstance.Connection;
            MySqlCommand command =
                new MySqlCommand("INSERT INTO LANGUAGE (ID, ABBREVATION, NAME) VALUES (@Id, @Abbrevation, @Name)",
                    connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@Id", language.Id));
            command.Parameters.Add(new MySqlParameter("@Abbrevation", language.Abbrevation));
            command.Parameters.Add(new MySqlParameter("@Name", language.Name));

            command.ExecuteNonQuery();

            language.Translations.ForEach(t => AddTranslation(t, language.Id));
        }

        /// <summary>
        /// Loads a language from the database.
        /// </summary>
        /// <param name="languageId">The id of the language to identify.</param>
        /// <returns>A language</returns>
        public Language LoadLanguage(int languageId)
        {
            Language language = null;

            MySqlConnection conneciton = Database.NewInstance.Connection;
            MySqlCommand command = new MySqlCommand(
                "SELECT ID, ABBREVATION, NAME FROM LANGUAGE WHERE ID = @languageId", conneciton)
            {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@languageId", languageId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string abbrevation = reader.GetString(1);
                    string name = reader.GetString(2);

                    language = new Language(id, abbrevation, name);
                }
            }

            LoadTranslations(languageId).ForEach(t => language?.AddTranslation(t));

            return language;
        }

        /// <summary>
        /// Deletes all rows from translation table.
        /// </summary>
        private void CleanTranslations()
        {
            MySqlConnection connection = Database.NewInstance.Connection;
            MySqlCommand command =
                new MySqlCommand("DELETE FROM TRANSLATION",
                    connection)
                {CommandType = CommandType.Text};

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes all rows from language table.
        /// </summary>
        private void CleanLanguages()
        {
            MySqlConnection connection = Database.NewInstance.Connection;
            MySqlCommand command =
                new MySqlCommand("DELETE FROM LANGUAGE",
                    connection)
                {CommandType = CommandType.Text};

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a translation to the database.
        /// </summary>
        /// <param name="translation">The translation to add.</param>
        /// <param name="languageId">The id of the language the translation belongs to.</param>
        private void AddTranslation(Translation translation, int languageId)
        {
            MySqlConnection connection = Database.NewInstance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "INSERT INTO TRANSLATION (ID, LANGUAGE_ID, TRANSLATION) VALUES (@Id, @languageId, @TranslationText)",
                    connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@Id", translation.Id));
            command.Parameters.Add(new MySqlParameter("@languageId", languageId));
            command.Parameters.Add(new MySqlParameter("@TranslationText", translation.TranslationText));

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Loads translations of a language from the database.
        /// </summary>
        /// <param name="languageId">The id of the language the translations belong to.</param>
        /// <returns>List of translations</returns>
        private List<Translation> LoadTranslations(int languageId)
        {
            List<Translation> translations = new List<Translation>();

            MySqlConnection conneciton = Database.NewInstance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, TRANSLATION FROM TRANSLATION WHERE LANGUAGE_ID = @languageId", conneciton)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@languageId", languageId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string translationText = reader.GetString(1);

                    translations.Add(new Translation(id, translationText));
                }
            }

            return translations;
        }
    }
}