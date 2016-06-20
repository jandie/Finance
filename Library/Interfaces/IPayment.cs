namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        string Name { get; }

        decimal Amount { get; }

        bool MayAddPayment { get; }

        decimal GetSum();

        decimal GetTotalAmount();
    }
}
