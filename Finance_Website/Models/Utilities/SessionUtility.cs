using System;
using Library.Classes;
using Library.Classes.Language;
using Repository;

namespace Finance_Website.Models.Utilities
{
    public class SessionUtility
    {
        public User User { get; set; }

        public Language Language { get; private set; }

        public string LastTab { get; set; }

        public string Email { private get; set; }

        public string Password { private get; set; }

        public void Refresh(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(LastTab) && lastTab != null)
                LastTab = lastTab;

            User = User == null
                ? DataRepository.Instance.Login(Email, Password)
                : DataRepository.Instance.LoadUser(Email);

            if (User == null)
            {
                Email = null;
                Password = null;

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

        public static SessionUtility InitializeUtil(Object util, string lastTab = null)
        {
            SessionUtility userUtility = util as SessionUtility ?? new SessionUtility();

            userUtility.Refresh(lastTab);

            return userUtility;
        }

        public static SessionUtility Login(string password, string email)
        {
            SessionUtility userUtility = new SessionUtility
            {
                Email = email,
                Password = password
            };

            userUtility.Refresh();

            return userUtility;
        }
    }
}