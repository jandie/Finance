using System;
using System.Collections.Generic;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes.Language;

namespace Repository
{
    public class LanguageRepository
    {
        private static LanguageRepository _instance;
        private readonly ILanguageContext _context;

        public static LanguageRepository Instance => _instance ?? (_instance = new LanguageRepository());

        private LanguageRepository()
        {
            _context = new LanguageSqlContext();
        }

        public void AddLanguages(List<Language> language)
        {
            try
            {
                _context.Clean();

                language.ForEach(l => _context.AddLaguage(l));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                 
                throw;
            }
        }

        public Language LoadLanguage(int id)
        {
            try
            {
                return _context.LoadLanguage(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw ;
            }
        }
    }
}
