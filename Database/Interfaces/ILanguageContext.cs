using Library.Classes.Language;

namespace Database.Interfaces
{
    public interface ILanguageContext
    {
        /// <summary>
        /// Deletes all rows from the language tables.
        /// </summary>
        void Clean();

        /// <summary>
        /// Adds a language to the database.
        /// </summary>
        /// <param name="language">The language to add.</param>
        void AddLaguage(Language language);

        /// <summary>
        /// Loads a language from the database.
        /// </summary>
        /// <param name="languageId">The id of the language to identify.</param>
        /// <returns>A language</returns>
        Language LoadLanguage(int languageId);
    }
}