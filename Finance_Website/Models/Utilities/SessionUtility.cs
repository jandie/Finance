using Library.Classes;
using Library.Classes.Language;
using Repository;

namespace Finance_Website.Models.Utilities
{
    public class SessionUtility
    {
        public User User { get; set; }

        public Language Language { get; set; }

        public string LastTab { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public void Refresh(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(LastTab) && lastTab != null)
                LastTab = lastTab;

            User = DataRepository.Instance.Login(Email, Password, true, true, true);

            if (User == null)
            {
                if (Language == null)
                {
                    Language = LoadAndAssignLanguage(1);
                }
            }
            else
            {
                if (Language == null || Language.Id != User.LanguageId)
                {
                    Language = LoadAndAssignLanguage(User.LanguageId);
                }
            }
        }

        private Language LoadAndAssignLanguage(int languageId)
        {
            Language = LanguageRepository.Instance.LoadLanguage(languageId);

            return Language;
        }
    }
}