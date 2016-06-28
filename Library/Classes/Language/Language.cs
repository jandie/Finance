using System.Collections.Generic;

namespace Library.Classes.Language
{
    public class Language
    {
        private List<Translation> _translations;

        public Language(int id, string abbrevation, string name)
        {
            Id = id;
            Abbrevation = abbrevation;
            Name = name;

            _translations = new List<Translation>();
        }

        public int Id { get; }

        public string Abbrevation { get; }

        public string Name { get; }

        public List<Translation> Translations => new List<Translation>(_translations);

        public void AddTranslation(Translation translation)
        {
            _translations.Add(translation);
        }

        public string GetText(int translationId)
        {
            string translationText = translationId.ToString();

            translationText = _translations.Find(t => t.Id == translationId)?.TranslationText;

            return translationText;
        }
    }
}
