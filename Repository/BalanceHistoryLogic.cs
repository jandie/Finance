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
            List<BalanceHistory> balanceHistories = new List<BalanceHistory>();

            DateTime lastMonth = DateTime.Today.AddMonths(-1);
            DateTime thisMonth = DateTime.Today;

            List<BalanceHistory> lastMonthBalanceHistory = _context.GetBalanceHistoriesOfMonth(user,
                lastMonth.ToString("yyyy-MM"), password);
            List<BalanceHistory> thisMonthBalanceHistory = _context.GetBalanceHistoriesOfMonth(user,
                thisMonth.ToString("yyyy-MM"), password);

            balanceHistories.AddRange(FillLastMonthsHistory(lastMonthBalanceHistory, 
                DetermineLastMonthEndBalanceHistory(lastMonthBalanceHistory, thisMonthBalanceHistory, user)));

            balanceHistories.AddRange(FillThisMonthsHistory(thisMonthBalanceHistory, balanceHistories[balanceHistories.Count - 1]));

            return balanceHistories;
        }

        private List<BalanceHistory> FillLastMonthsHistory(IReadOnlyList<BalanceHistory> lastMonthBalanceHistory, BalanceHistory endBalanceHistory)
        {
            List<BalanceHistory> balanceHistories = new List<BalanceHistory>();

            if (lastMonthBalanceHistory.Count == 0)
            {
                balanceHistories.Add(endBalanceHistory);

                return balanceHistories;
            }

            BalanceHistory prevHistory = lastMonthBalanceHistory[0];
            balanceHistories.Add(lastMonthBalanceHistory[0]);

            foreach (BalanceHistory balanceHistory in lastMonthBalanceHistory)
            {
                balanceHistories.AddRange(FillBetweenHistories(prevHistory, balanceHistory));

                prevHistory = balanceHistory;
            }

            if (balanceHistories[balanceHistories.Count - 1].DateTime.Day != endBalanceHistory.DateTime.Day)
            {
                balanceHistories.AddRange(FillBetweenHistories(prevHistory, endBalanceHistory));
            }

            return balanceHistories;
        }

        private BalanceHistory DetermineLastMonthEndBalanceHistory(List<BalanceHistory> lastMonthHistory,
            List<BalanceHistory> thisMonythHistory, User user)
        {
            DateTime lastMonth = DateTime.Today.AddMonths(-1);

            int days = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);

            DateTime lastDayInMonth = new DateTime(lastMonth.Year, lastMonth.Month, days);

            if (lastMonthHistory.Count != 0) return new BalanceHistory(-1, lastMonthHistory[lastMonthHistory.Count - 1].Amount, null)
            {
                DateTime = lastDayInMonth
            };

            if (thisMonythHistory.Count == 0)
            {
                return new BalanceHistory(-1, user.TotalBalance, null)
                {
                    DateTime = lastDayInMonth
                };
            }

            return new BalanceHistory(-1, thisMonythHistory[0].Amount, null)
            {
                DateTime = lastDayInMonth
            };
        } 

        private List<BalanceHistory> FillThisMonthsHistory(List<BalanceHistory> thisMonthBalanceHistory, BalanceHistory lastMonthEndBalanceHistory)
        {
            List<BalanceHistory> balanceHistories = new List<BalanceHistory>();

            if (thisMonthBalanceHistory.Count == 0)
            {
                BalanceHistory todayHistory = new BalanceHistory(-1, lastMonthEndBalanceHistory.Amount, null)
                {
                    DateTime = DateTime.Today
                };

                balanceHistories.AddRange(FillBetweenHistories(lastMonthEndBalanceHistory, todayHistory));

                return balanceHistories;
            }

            if (thisMonthBalanceHistory[thisMonthBalanceHistory.Count - 1].DateTime.Day != DateTime.Today.Day)
            {
                BalanceHistory todayHistory = new BalanceHistory(-1, thisMonthBalanceHistory[thisMonthBalanceHistory.Count - 1].Amount, null);

                thisMonthBalanceHistory.Add(todayHistory);
            }

            BalanceHistory prevHistory = lastMonthEndBalanceHistory;

            foreach (BalanceHistory balanceHistory in thisMonthBalanceHistory)
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
