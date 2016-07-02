using Library.Classes;
using Library.Classes.Language;
using Repository;

namespace Finance_Website.Models.Utilities
{
    public class UserUtility
    {
        public UserUtility()
        {
            User = null;
            Language = null;
            LastTab = null;
        }

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

        public User User { get; set; }

        public Language Language { get; set; }

        public string LastTab { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Exception { get; set; }

        public string Message { get; set; }
    }
}