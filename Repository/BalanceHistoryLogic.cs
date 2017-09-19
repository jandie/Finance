using System;
using System.Collections.Generic;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class BalanceHistoryLogic
    {
        private readonly IBalanceHistoryContext _context;

        public BalanceHistoryLogic(Database.Database database)
        {
            _context = new BalanceHistorySqlContext(database);
        }

        public void UpdateBalance(User user, string password)
        {
            try
            {
                BalanceHistory balanceHistory = _context.UpdateBalanceHistory(user, password);

                user.DeleteBalanceHistory(balanceHistory.DateTime);
                user.AddBalanceHistory(balanceHistory);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public List<BalanceHistory> GetBalanceHistoryOfUser(User user, string password)
        {
            User dummyUser = new User(-1, "", "", "", -1, null, "", "");

            List<BalanceHistory> balanceHistory = _context.GetBalanceHistoriesOfMonth(user,
                DateTime.Now.AddDays(-62), password);

            balanceHistory.ForEach(dummyUser.AddBalanceHistory);

            dummyUser.AddBalanceHistory(new BalanceHistory(-1, user.TotalBalance, ""));

            if (dummyUser.BalanceHistories.Count > 1)
            {
                dummyUser.BalanceHistories = FillHistory(dummyUser.BalanceHistories);
            }

            return dummyUser.BalanceHistories;
        }

        private List<BalanceHistory> FillHistory(IReadOnlyList<BalanceHistory> balanceHistoryList)
        {
            List<BalanceHistory> balanceHistories = new List<BalanceHistory>();

            BalanceHistory prevHistory = balanceHistoryList[0];
            balanceHistories.Add(balanceHistoryList[0]);

            //Find gaps and fill them
            foreach (BalanceHistory balanceHistory in balanceHistoryList)
            {
                balanceHistories.AddRange(FillBetweenHistories(prevHistory, balanceHistory));

                prevHistory = balanceHistory;
            }

            return balanceHistories;
        }

        private List<BalanceHistory> FillBetweenHistories(BalanceHistory begin, BalanceHistory end)
        {
            List<BalanceHistory> balanceHistories = new List<BalanceHistory> {begin};

            if ((end.DateTime - balanceHistories[balanceHistories.Count - 1].DateTime).TotalDays < 1)
            {
                return new List<BalanceHistory>();
            }

            while (!((end.DateTime - balanceHistories[balanceHistories.Count - 1].DateTime).TotalDays >= 1 && 
                (end.DateTime - balanceHistories[balanceHistories.Count - 1].DateTime).TotalDays < 2))
            {
                BalanceHistory newBalanceHistory = new BalanceHistory(-1, balanceHistories[balanceHistories.Count - 1].Amount, null)
                {
                    DateTime = balanceHistories[balanceHistories.Count - 1].DateTime.AddDays(1)
                };

                balanceHistories.Add(newBalanceHistory);
            }

            balanceHistories.Add(end);

            balanceHistories.Remove(begin);

            return balanceHistories;
        }
    }
}
