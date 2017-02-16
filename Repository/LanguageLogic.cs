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
            finally
            {
                _context.CloseDb();
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

                throw;
            }
            finally
            {
                _context.CloseDb();
            }
        }
    }
}