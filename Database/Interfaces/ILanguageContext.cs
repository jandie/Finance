using Library.Classes.Language;

namespace Database.Interfaces
{
    public interface ILanguageContext
    {
        void Clean();

        void AddLaguage(Language language);

        Language LoadLanguage(int languageId);
    }
}