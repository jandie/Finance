namespace Database.Interfaces
{
    public interface IChangeContext
    {
        void ChangeBalance(int id, string name, decimal balanceAmount);

        void ChangePayment(int id, string name, decimal amount);

        void ChangeTransaction(int id, decimal amount, string description);
    }
}
