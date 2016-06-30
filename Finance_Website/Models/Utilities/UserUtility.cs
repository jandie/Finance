using Library.Classes;
using Library.Classes.Language;
using Repository;

namespace Finance_Website.Models.Utilities
{
    public class UserUtility
    {
        public UserUtility(ref object sessionUser, ref object sessionPassword, ref object sessionLanguage, ref object sessionLastTab, string lastTab)
        {
            if (string.IsNullOrWhiteSpace(sessionLastTab as string))
                sessionLastTab = lastTab;

            User = null;

            User = sessionUser as User;

            if (User == null)
            {
                if (!(sessionLanguage is Language))
                {
                    sessionLanguage = LoadAndAssignLanguage(1);
                }
                else
                {
                    Language = (Language) sessionLanguage;
                }
            } else if (!(sessionLanguage is Language))
            {
                sessionLanguage = LoadAndAssignLanguage(User.LanguageId);
            }
            else
            {
                Language = (Language) sessionLanguage;

                if (Language.Id != User.LanguageId)
                {
                    sessionLanguage = LoadAndAssignLanguage(User.LanguageId);
                }
            }

            User = DataRepository.Instance.Login(User?.Email, sessionPassword as string, true, true, true);
        }

        private object LoadAndAssignLanguage(int languageId)
        {
            Language = LanguageRepository.Instance.LoadLanguage(languageId);

            return Language;
        }

        public User User { get;}

        public Language Language { get; private set; }
    }
}