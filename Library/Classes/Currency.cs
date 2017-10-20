namespace Library.Classes
{
    public class Currency
    {
        /// <summary>
        /// Creates an instance of the Currency object.
        /// </summary>
        /// <param name="id">The ID of the Currency.</param>
        /// <param name="abbrevation">The abbrevation of the Currency.</param>
        /// <param name="name">The name of the Currency.</param>
        /// <param name="html">The HTML of the Currency.</param>
        public Currency(int id, string abbrevation, string name, string html)
        {
            Id = id;
            Abbrevation = abbrevation;
            Name = name;
            Html = html;
        }

        /// <summary>
        /// The ID of the Currency.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The abbrevation of the Currency.
        /// </summary>
        public string Abbrevation { get; }

        /// <summary>
        /// The name of the Currency.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The HTML of the Currency.
        /// </summary>
        public string Html { get; }
    }
}