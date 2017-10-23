using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using FinanceLibrary.Classes;
using FinanceLibrary.Repository.Interfaces;
using FinanceLibrary.Utils;

namespace FinanceLibrary.Repository.SqlContexts
{
    public class BalanceHistorySqlContext : IBalanceHistoryContext
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public BalanceHistorySqlContext(Database db)
        {
            _db = db;
            _encryption = new Encryption();
        }

        /// <summary>
        /// Updates the balance history of the user to the new balance sum.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The new BalanceHistory with the updated value.</returns>
        public BalanceHistory UpdateBalanceHistory(User user)
        {
            try
            {
                int exsistingId = CheckExistingBalanceHistory(user);

                if (exsistingId < 0)
                    return AddBalanceHistory(user);

                const string query = "UPDATE `balancehistory` SET `BankAccountHistory` = @totalBalance, `BankAccountHistorySalt` = @totalBalanceSalt WHERE `ID` = @id";
                string totalBalanceSalt = Hashing.ExtractSalt(Hashing.CreateHash(user.TotalBalance.ToString(CultureInfo.InvariantCulture)));
                string encryptedTotalBalance =
                    _encryption.EncryptText(user.TotalBalance.ToString(CultureInfo.InvariantCulture), user.MasterPassword,
                        totalBalanceSalt);
                BalanceHistory balanceHistory = new BalanceHistory(-1, user.TotalBalance, totalBalanceSalt);
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"totalBalance", encryptedTotalBalance },
                    {"totalBalanceSalt", totalBalanceSalt },
                    {"id", exsistingId },
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);

                return balanceHistory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Checks if the user has balance history for the current day.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>The id of the exsisting balancehistory or -1 if no record was found.</returns>
        private int CheckExistingBalanceHistory(User user)
        {
            try
            {
                int id = -1;
                const string query = "SELECT ID FROM balancehistory WHERE UserId = @userId AND Date LIKE (@month)";

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"userId",  user.Id},
                    {"month",  $"%{DateTime.Now:yyyy-MM-dd}%"},
                };

                using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
                {
                    if (table != null && table.Rows.Count > 0)
                        id = Convert.ToInt32(table.Rows[0][0]);
                }

                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Adds new balance history to the user.
        /// </summary>
        /// <param name="user">The user to add the balancehistory to.</param>
        /// <returns></returns>
        private BalanceHistory AddBalanceHistory(User user)
        {
            try
            {
                const string query = 
                    "INSERT INTO `balancehistory` (`UserId`, `BankAccountHistory`, `BankAccountHistorySalt`, `Date`) " +
                                     "VALUES (@userId, @totalBalance, @totalBalanceSalt, @date)";

                string totalBalanceSalt = Hashing.ExtractSalt(Hashing.CreateHash(user.TotalBalance.ToString(CultureInfo.InvariantCulture)));
                string encryptedTotalBalance =
                    _encryption.EncryptText(user.TotalBalance.ToString(CultureInfo.InvariantCulture), user.MasterPassword,
                        totalBalanceSalt);

                BalanceHistory balanceHistory = new BalanceHistory(-1, user.TotalBalance, totalBalanceSalt);
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"userId" , user.Id},
                    {"totalBalance",  encryptedTotalBalance},
                    {"totalBalanceSalt",  totalBalanceSalt},
                    {"date",  balanceHistory.DateTime.ToString("yyyy-MM-dd")},
                };

                long id = (long) _db.Execute(query, parameters, Database.QueryType.Insert);

                balanceHistory.Id = Convert.ToInt32(id);

                return balanceHistory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get all balancehistory objects of a specific month.
        /// </summary>
        /// <param name="user">The user to add the balancehistories to.</param>
        /// <param name="month">The month to get the objects from.</param>
        /// <param name="password">The password for encryption.</param>
        /// <returns>List of balancehistory objects.</returns>
        public List<BalanceHistory> GetBalanceHistoriesOfMonth(User user, DateTime month)
        {
            try
            {
                const string query = "SELECT ID, BankAccountHistory, BankAccountHistorySalt, Date FROM balancehistory WHERE UserId = @userId AND Date > @month";
                List<BalanceHistory> balanceHistories = new List<BalanceHistory>();

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"userId", user.Id},
                    {"month", month}
                };

                using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
                {
                    if (table == null) return balanceHistories;
                    foreach (DataRow row in table.Rows)
                    {
                        int id = Convert.ToInt32(row[0]);
                        string encryptedBalance = Convert.ToString(row[1]);
                        string balanceSalt = Convert.ToString(row[2]);
                        DateTime dateTime = Convert.ToDateTime(row[3]);

                        decimal decryptedBalance =
                            Convert.ToDecimal(_encryption.DecryptText(encryptedBalance, user.MasterPassword, balanceSalt));

                        BalanceHistory balanceHistory = new BalanceHistory(id, decryptedBalance, balanceSalt)
                        {
                            DateTime = dateTime
                        };

                        balanceHistories.Add(balanceHistory);
                    }
                }

                return balanceHistories;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
