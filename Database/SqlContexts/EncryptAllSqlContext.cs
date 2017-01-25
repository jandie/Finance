using System;
using System.Collections.Generic;
using System.Data;
using Library.Classes;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class EncryptAllSqlContext
    {
        public void EncryptUserData(int userId, string email, string password, string salt)
        {
            EncryptBankAccountData(userId, password, salt);
            EncryptPaymentData(userId, password, salt);

            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML, U.PASSWORD, U.ENCRYPTED FROM USER U " +
                    "INNER JOIN CURRENCY C ON C.ID = U.CURRENCY WHERE EMAIL = @email",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@email", email));

            int currencyId;
            int languageId;
            string lastName;
            string name;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    reader.Close();

                    throw new WrongUsernameOrPasswordException();
                }

                int id = reader.GetInt32(0);
                name = reader.GetString(1);
                lastName = reader.GetString(2);
                languageId = reader.GetInt32(3);
                currencyId = reader.GetInt32(4);

                reader.Close();
            }

            ChangeSqlContext c = new ChangeSqlContext();

            c.ChangeUser(name, lastName, email, currencyId, languageId, password, salt);
        }

        private void EncryptBankAccountData(int userId, string password, string salt)
        {
            List<Balance> bankAccounts = new List<Balance>();

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, BALANCE, NAME FROM BANKACCOUNT WHERE USER_ID = @userId",
                    conneciton)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    decimal balance = reader.GetDecimal(1);
                    string name = reader.GetString(2);

                    bankAccounts.Add(new Balance(id, name, balance));
                }
            }

            ChangeSqlContext c = new ChangeSqlContext();

            foreach (Balance balance in bankAccounts)
            {
                c.ChangeBalance(balance, password);
            }
        }

        private void EncryptPaymentData(int userId, string password, string salt)
        {
            List<IPayment> payments = new List<IPayment>();
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, NAME, AMOUNT, TYPE FROM PAYMENT WHERE USER_ID = @userId",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal amount = reader.GetDecimal(2);
                    PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), reader.GetString(3));

                    switch (type)
                    {
                        case PaymentType.MonthlyBill:
                            payments.Add(new MonthlyBill(id, name, amount, type));
                            break;
                        case PaymentType.MonthlyIncome:
                            payments.Add(new MonthlyIncome(id, name, amount, type));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            ChangeSqlContext c = new ChangeSqlContext();

            foreach (IPayment payment in payments)
            {
                c.ChangePayment(payment as Payment, password);

                EncryptTransactions(payment, password, salt);
            }
        }

        private void EncryptTransactions(IPayment payment, string password, string salt)
        {
            List<Transaction> transactions = new List<Transaction>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT ID, AMOUNT, DESCRIPTION FROM TRANSACTION WHERE PAYMENT_ID = @paymentId",
                    connecion)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@paymentId", payment.Id));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    decimal amount = reader.GetDecimal(1);
                    string description = reader.GetString(2);

                    if (payment is MonthlyBill)
                        transactions.Add(new Transaction(id, amount, description, false));
                    else if (payment is MonthlyIncome)
                        transactions.Add(new Transaction(id, amount, description, true));
                }
            }

            ChangeSqlContext c = new ChangeSqlContext();

            foreach (Transaction transaction in transactions)
            {
                c.ChangeTransaction(transaction, password);   
            }
        }
    }
}
