using System;
using System.Data;
using System.Globalization;
using Database.Interfaces;
using Library.Classes;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class BalanceHistorySqlContext : IBalanceHistoryContext, IDatabaseClosable
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public BalanceHistorySqlContext(Database db = null)
        {
            _db = db ?? new Database();
            _encryption = new Encryption();
        }

        public BalanceHistory UpdateBalanceHistory(User user, string password)
        {
            try
            {
                int exsistingId = CheckExistingBalanceHistory(user);

                if (exsistingId < 0)
                {
                    return AddBalanceHistory(user, password);
                }

                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("UPDATE `balancehistory` SET `BankAccountHistory` = @totalBalance, " +
                                     "`BankAccountHistorySalt` = @totalBalanceSalt WHERE `ID` = @id",
                            connection)
                        { CommandType = CommandType.Text };

                string totalBalanceSalt = Hashing.ExtractSalt(Hashing.CreateHash(user.TotalBalance.ToString(CultureInfo.InvariantCulture)));
                string encryptedTotalBalance =
                    _encryption.EncryptText(user.TotalBalance.ToString(CultureInfo.InvariantCulture), password,
                        totalBalanceSalt);

                BalanceHistory balanceHistory = new BalanceHistory(-1, user.TotalBalance, totalBalanceSalt);

                command.Parameters.Add(new MySqlParameter("@totalBalance", encryptedTotalBalance));
                command.Parameters.Add(new MySqlParameter("@totalBalanceSalt", totalBalanceSalt));
                command.Parameters.Add(new MySqlParameter("@id", exsistingId));

                command.ExecuteNonQuery();

                return balanceHistory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private int CheckExistingBalanceHistory(User user)
        {
            try
            {
                int id = -1;

                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("SELECT ID FROM balancehistory WHERE UserId = @userId AND Date LIKE @month)",
                            connection)
                        { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@userId", user.Id));
                command.Parameters.Add(new MySqlParameter("@month", $"%{DateTime.Now:yyyy-MM-dd}%"));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }

                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private BalanceHistory AddBalanceHistory(User user, string password)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("INSERT INTO `balancehistory` (`UserId`, `BankAccountHistory`, `BankAccountHistorySalt`, `Date`) " +
                                     "VALUES (@userId, @totalBalance, @totalBalanceSalt, @date)",
                            connection)
                        { CommandType = CommandType.Text };

                string totalBalanceSalt = Hashing.ExtractSalt(Hashing.CreateHash(user.TotalBalance.ToString(CultureInfo.InvariantCulture)));
                string encryptedTotalBalance =
                    _encryption.EncryptText(user.TotalBalance.ToString(CultureInfo.InvariantCulture), password,
                        totalBalanceSalt);

                BalanceHistory balanceHistory = new BalanceHistory(-1, user.TotalBalance, totalBalanceSalt);

                command.Parameters.Add(new MySqlParameter("@userId", user.Id));
                command.Parameters.Add(new MySqlParameter("@totalBalance", encryptedTotalBalance));
                command.Parameters.Add(new MySqlParameter("@totalBalanceSalt", totalBalanceSalt));
                command.Parameters.Add(new MySqlParameter("@date", balanceHistory.DateTime.ToString("yyyy-MM-dd")));

                command.ExecuteNonQuery();

                long id = command.LastInsertedId;

                balanceHistory.Id = Convert.ToInt32(id);

                return balanceHistory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CloseDb()
        {
            _db.Close();
        }
    }
}
