﻿using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes.Language;
using Repository.Exceptions;

namespace Repository
{
    public class ChangeRepository
    {
        private static ChangeRepository _instance;
        private readonly IChangeContext _context;

        public ChangeRepository()
        {
            _context = new ChangeSqlContext();
        }

        public static ChangeRepository Instance => _instance ?? (_instance = new ChangeRepository());

        public void ChangeBalance(int id, string name, decimal balanceAmount)
        {
            try
            {
                _context.ChangeBalance(id, name, balanceAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangePayment(int id, string name, decimal amount)
        {
            try
            {
                _context.ChangePayment(id, name, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangeTransaction(int id, decimal amount, string description)
        {
            try
            {
                _context.ChangeTransaction(id, amount, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangeUser(string name, string lastName, string email, int currencyId, int languageId,
            string currentPassword, string newPassword, string repeatedPassword, Language language)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new UserChangeException(language.GetText(35));

                if (string.IsNullOrWhiteSpace(lastName))
                    throw new UserChangeException(language.GetText(36));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Length < 8)
                    throw new UserChangeException(language.GetText(39));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Contains(" "))
                    throw new UserChangeException(language.GetText(40));

                if (DataRepository.Instance.Login(email, currentPassword, false, false, false) == null)
                    throw new UserChangeException(language.GetText(33));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != repeatedPassword)
                    throw new UserChangeException(language.GetText(41));

                _context.ChangeUser(name, lastName, email, currencyId, languageId);

                if (string.IsNullOrWhiteSpace(newPassword)) return;

                _context.ChangePassword(email, newPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}