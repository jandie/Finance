using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Library.Classes.Language;
using Newtonsoft.Json.Linq;
using Repository;

namespace Language_import
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            const string path = "E:\\BSync\\Rest\\Projects\\Financial management app\\Finance\\Language_import\\JSON file\\Translations.json";
            //const string path = "C:\\Users\\Jandie\\BitTorrent Sync\\Bsync\\Rest\\Projects\\Financial management app\\Finance\\Language_import\\JSON file\\Translations.json";
            string json = "";

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadToEnd();
                    json += line;
                    Console.WriteLine(line);
                }

                Console.WriteLine($"File read! ({stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            List<Language> languages = new List<Language>();

            try
            {
                dynamic parsedData = JObject.Parse(json);

                int languageId = 0;

                foreach (dynamic language in parsedData.Languages)
                {
                    languages.Add(new Language(languageId, language.Abbrevation.ToString(), language.Name.ToString()));

                    languageId ++;
                }

                foreach (dynamic translation in parsedData.Translations)
                {
                    int translationsCount = 0;

                    foreach (dynamic t in translation.Translations)
                    {
                        translationsCount++;
                    }

                    for (int i = 0; i < translationsCount; i++)
                    {
                        languages[i].AddTranslation(new Translation(Convert.ToInt32(translation.ID), translation.Translations[i].ToString()));
                    }
                }

                Console.WriteLine($"Json file converted! ({stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be converted into library:");
                Console.WriteLine(e.Message);
            }

            try
            {
                LanguageRepository.Instance.AddLanguages(languages);

                Console.WriteLine($"Languages saved to database! ({stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The languages could not be saved to the database.");
                Console.WriteLine(e.Message);
            }

            stopwatch.Stop();

            Console.WriteLine($"Done! ({stopwatch.ElapsedMilliseconds}ms)");

            Console.ReadLine();
        }
    }
}
