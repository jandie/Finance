namespace Library.Classes.Language
{
    public class Translation
    {
        public Translation(int id, string translationText)
        {
            Id = id;
            TranslationText = translationText;
        }

        public int Id { get; }

        public string TranslationText { get; }
    }
}