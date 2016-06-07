using Library.Enums;

namespace Database.Interfaces
{
    public interface IInsertContext
    {
        void AddBankAccount(int userId, string name, decimal balance);
        void AddPayment(int userId, string name, decimal amount, PaymentType type);
        void AddTransaction(int paymentId, decimal amount, string description);
    }
}
