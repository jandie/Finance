using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes.Language;

namespace Repository
{
    public class LanguageRepository
    {
        private static LanguageRepository _instance;
        private readonly ILanguageContext _context;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private LanguageRepository()
        {
            _context = new LanguageSqlContext();
        }

        public static LanguageRepository Instance => _instance ?? (_instance = new LanguageRepository());

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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