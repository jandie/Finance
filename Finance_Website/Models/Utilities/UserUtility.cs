using Library.Classes;

namespace Finance_Website.Models.Utilities
{
    public class UserUtility
    {
        public static bool UserIsValid(User user)
        {
            return user != null;
        }

        private UserUtility()
        {
            
        }
    }
}