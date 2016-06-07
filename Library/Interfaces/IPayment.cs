namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        bool MayAddPayment { get; }

        decimal GetSum();
    }
}
