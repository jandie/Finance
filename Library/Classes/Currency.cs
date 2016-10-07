namespace Library.Classes
{
    public class Currency
    {
        public Currency(int id, string abbrevation, string name, string html)
        {
            Id = id;
            Abbrevation = abbrevation;
            Name = name;
            Html = html;
        }

        public int Id { get; }

        public string Abbrevation { get; }

        public string Name { get; }

        public string Html { get; }
    }
}