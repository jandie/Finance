using System;
using Library.Classes;
using Repository;

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