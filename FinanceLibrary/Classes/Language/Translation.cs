namespace FinanceLibrary.Classes.Language
{
    public class Translation
    {
        /// <summary>
        /// Creates an instance of the Translation object.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <param name="translationText">The text of the translation.</param>
        public Translation(int id, string translationText)
        {
            Id = id;
            TranslationText = translationText;
        }

        /// <summary>
        /// The id of the translation.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The text of the translation.
        /// </summary>
        public string TranslationText { get; }
    }
}