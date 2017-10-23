using System;
using System.Collections.Generic;
using FinanceLibrary.Classes;

namespace FinanceLibrary.Repository.Interfaces
{
    public interface IBalanceHistoryContext
    {
        /// <summary>
        /// Updates the balance history of the user to the new balance sum.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The new BalanceHistory with the updated value.</returns>
        BalanceHistory UpdateBalanceHistory(User user);

        /// <summary>
        /// Get all balancehistory objects of a specific month.
        /// </summary>
        /// <param name="user">The user to add the balancehistories to.</param>
        /// <param name="month">The month to get the objects from.</param>
        /// <returns>List of balancehistory objects.</returns>
        List<BalanceHistory> GetBalanceHistoriesOfMonth(User user, DateTime month);
    }
}
