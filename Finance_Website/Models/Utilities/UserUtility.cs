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
                    Language = LanguageRepository.Instance.LoadLanguage(0);

                    sessionLanguage = Language;
                }
                else
                {
                    Language = (Language) sessionLanguage;
                }
            }

            if (!(sessionLanguage is Language))
            {
                Language = LanguageRepository.Instance.LoadLanguage(0);

                sessionLanguage = Language;
            }
            else
            {
                Language = (Language) sessionLanguage;
            }

            User = DataRepository.Instance.Login(User?.Email, sessionPassword as string, true, true, true);
        }

        public User User { get;}

        public Language Language { get; }
    }
}