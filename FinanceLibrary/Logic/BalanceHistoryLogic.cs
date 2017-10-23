using System;
using System.Collections.Generic;
using FinanceLibrary.Classes;
using FinanceLibrary.Repository;
using FinanceLibrary.Repository.Interfaces;
using FinanceLibrary.Repository.SqlContexts;

namespace FinanceLibrary.Logic
{
    public class BalanceHistoryLogic
    {
        private readonly IBalanceHistoryContext _context;

        public BalanceHistoryLogic(Database database)
        {
            _context = new BalanceHistorySqlContext(database);
        }

        /// <summary>
        /// Updates the Balance of an User.
        /// </summary>
        /// <param name="user">The User to update the Balance from.</param>
        public void UpdateBalance(User user)
        {
            try
            {
                BalanceHistory balanceHistory = _context.UpdateBalanceHistory(user);

                user.DeleteBalanceHistory(balanceHistory.DateTime);
                user.AddBalanceHistory(balanceHistory);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// Gets recent balancehistory of the User.
        /// </summary>
        /// <param name="user">The User to get the history from.</param>
        /// <returns>Recent balancehistory.</returns>
        public List<BalanceHistory> GetBalanceHistoryOfUser(User user)
        {
            User dummyUser = new User(-1, "", "", "", -1, null, "", "");

            List<BalanceHistory> balanceHistory = _context.GetBalanceHistoriesOfMonth(user,
                DateTime.Now.AddDays(-62));

            balanceHistory.ForEach(dummyUser.AddBalanceHistory);

            dummyUser.AddBalanceHistory(new BalanceHistory(-1, user.TotalBalance, ""));

            if (dummyUser.BalanceHistories.Count > 1)
            {
                dummyUser.BalanceHistories = FillHistory(dummyUser.BalanceHistories);
            }

            return dummyUser.BalanceHistories;
        }

        /// <summary>
        /// Fills the gaps in the List of balancehistories.
        /// </summary>
        /// <param name="balanceHistoryList">The list of balancyhistories.</param>
        /// <returns>A new list of balancehistories with the gaps filed.</returns>
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

        /// <summary>
        /// Fills hsitory between 2 balancehistories.
        /// </summary>
        /// <param name="begin">The first balancehistory.</param>
        /// <param name="end">The last balancehistory.</param>
        /// <returns>History between the 2 histories including the begin and end history.</returns>
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
