using Database.SqlContexts;

namespace ManualEncryptionConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptAllSqlContext c = new EncryptAllSqlContext();

            c.EncryptUserData(10, "michel@live.nl", "goes1995", "dYZR4rXWBgmBghR8wR7chQ==");
        }
    }
}
