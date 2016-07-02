namespace Database.Interfaces
{
    public interface IDeleteContext
    {
        void DeleteBalance(int id);

        void DeletePayment(int id);

        void DeleteTransaction(int id);
    }
}