using System;
using System.Collections.Generic;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes.Language;

namespace Repository
{
    public class LanguageLogic
    {
        private readonly ILanguageContext _context;

        public LanguageLogic()
        {
            _context = new LanguageSqlContext();
        }
        
        /// <summary>
        /// Cleans all languages and adds a list of new ones.
        /// </summary>
        /// <param name="languages">List of new languages.</param>
        public void AddLanguages(List<Language> languages)
        {
            try
            {
                _context.Clean();

                languages.ForEach(l => _context.AddLaguage(l));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }

        /// <summary>
        /// Loads a loanguage based on the id.
        /// </summary>
        /// <param name="id">The id of the language.</param>
        /// <returns>The language that was found.</returns>
        public Language LoadLanguage(int id)
        {
            try
            {
                return _context.LoadLanguage(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}