using System.Collections.Generic;

namespace FinanceLibrary.Classes.Language
{
    public class Language
    {
        /// <summary>
        /// List of Translations of the Language.
        /// </summary>
        private readonly List<Translation> _translations;

        /// <summary>
        /// Creates an instance of the Language object.
        /// </summary>
        /// <param name="id">The ID of the Language.</param>
        /// <param name="abbrevation">The abbrevation of the Language.</param>
        /// <param name="name">The name of the Language.</param>
        public Language(int id, string abbrevation, string name)
        {
            Id = id;
            Abbrevation = abbrevation;
            Name = name;

            _translations = new List<Translation>();
        }

        /// <summary>
        /// The ID of the Language.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The abbrevation of the Language.
        /// </summary>
        public string Abbrevation { get; }

        /// <summary>
        /// The name of the Language.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a copy of the translation list of the Language.
        /// </summary>
        public List<Translation> Translations => new List<Translation>(_translations);

        /// <summary>
        /// Adds a translation to the language.
        /// </summary>
        /// <param name="translation">The translaction to add.</param>
        public void AddTranslation(Translation translation)
        {
            _translations.Add(translation);
        }

        /// <summary>
        /// Gets text of a language translation.
        /// </summary>
        /// <param name="translationId">The ID of the translation to get.</param>
        /// <returns>Text of a language translation.</returns>
        public string GetText(int translationId)
        {
            string translationText = _translations.Find(t => t.Id == translationId)?.TranslationText;

            return translationText;
        }
    }
}