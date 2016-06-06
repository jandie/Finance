namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        decimal GetSum();
    }
}
