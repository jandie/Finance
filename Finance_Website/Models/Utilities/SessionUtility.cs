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

        private string Email { get; set; }

        public string Password { get; private set; }

        private void Refresh(string lastTab = null)
        {
            if (string.IsNullOrWhiteSpace(LastTab) && lastTab != null)
                LastTab = lastTab;

            User = User == null
                ? new DataRepository().Login(Email, Password)
                : new DataRepository().CheckUser(User);

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
            Language = new LanguageRepository().LoadLanguage(languageId);

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