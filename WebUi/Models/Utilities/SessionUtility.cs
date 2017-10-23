using System;
using FinanceLibrary.Classes;
using FinanceLibrary.Classes.Language;
using FinanceLibrary.Logic;

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
                ? new DataLogic().Login(Email, Password)
                : new DataLogic().CheckUser(User, Password);

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
            Language = new LanguageLogic().LoadLanguage(languageId);

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