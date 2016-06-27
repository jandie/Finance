using System.Collections.Generic;
using System.Data;
using Library.Classes.Language;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class LanguageSqlContext
    {
        public void Clean()
        {
            CleanTranslations();
            CleanLanguages();
        }

        public void CleanTranslations()
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("DELETE FROM TRANSLATION",
                    connection)
                { CommandType = CommandType.Text };

            command.ExecuteNonQuery();
        }

        public void CleanLanguages()
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("DELETE FROM LANGUAGE",
                    connection)
                { CommandType = CommandType.Text };

            command.ExecuteNonQuery();
        }

        public void AddLaguage(Language language)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("INSERT INTO LANGUAGE (ID, ABBREVATION, NAME) VALUES (@Id, @Abbrevation, @Name)",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@Id", language.Id));
            command.Parameters.Add(new MySqlParameter("@Abbrevation", language.Abbrevation));
            command.Parameters.Add(new MySqlParameter("@Name", language.Name));

            command.ExecuteNonQuery();

            language.Translations.ForEach(t => AddTranslation(t, language.Id));
        }

        public void AddTranslation(Translation translation, int languageId)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("INSERT INTO LANGUAGE (ID, LANGUAGE_ID, TRANSLATION) VALUES (@Id, @languageId, @TranslationText)",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@Id", translation.Id));
            command.Parameters.Add(new MySqlParameter("@languageId", languageId));
            command.Parameters.Add(new MySqlParameter("@TranslationText", translation.TranslationText));

            command.ExecuteNonQuery();
        }

        public Language LoadLanguage(int languageId)
        {
            Language language = null;

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, ABBREVATION, NAME FROM LANGUAGE WHERE ID = @languageId", conneciton)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@languageId", languageId));

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string abbrevation = reader.GetString(1);
                string name = reader.GetString(2);

                language = new Language(id, abbrevation, name);
            }

            reader.Close();

            return language;
        }

        public List<Translation> LoadTranslations(int languageId) 
        {
            List<Translation> translations = new List<Translation>();

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, TRANSLATION FROM TRANSLATION WHERE LANGUAGE_ID = @languageId", conneciton)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@languageId", languageId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string translationText = reader.GetString(1);

                translations.Add(new Translation(id, translationText));
            }

            reader.Close();

            return translations;
        }
    }
}
