using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Database;
using Library.Classes.Language;
using Newtonsoft.Json.Linq;
using Repository;

namespace Language_import
{
    internal class Program
    {
        private static string _hashCollection;

        [STAThread]
        private static void Main(string[] args)
        {
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        UpdateLanguage();
                        break;
                    case "2":
                        CreateHash(Console.ReadLine());
                        break;
                    case "3":
                        TestValidate(Console.ReadLine(), _hashCollection);
                        break;
                    default:
                        UpdateLanguage();
                        break;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void CreateHash(string password)
        {
            _hashCollection = Hashing.CreateHash(password);

            Clipboard.SetText(_hashCollection);

            Console.WriteLine(_hashCollection);
        }

        private static void TestValidate(string password, string hashCollection)
        {
            for (int i = 0; i < 101; i++)
            {
                Stopwatch s = new Stopwatch();
                s.Start();

                Console.WriteLine(Hashing.ValidatePassword(password, hashCollection));

                s.Stop();

                Console.WriteLine($"It took me {s.Elapsed.Milliseconds} ms to check");
            }
        }

        private static void UpdateLanguage()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            const string path =
                "E:\\BSync\\Rest\\Projects\\Financial management app\\Finance\\Language_import\\JSON file\\Translations.json";
            //const string path = "C:\\Users\\Jandie\\BitTorrent Sync\\Bsync\\Rest\\Projects\\Financial management app\\Finance\\Language_import\\JSON file\\Translations.json";
            string json = "";

            try
            {
                // Open the text file using a stream reader.
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

                    languageId++;
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
                        languages[i].AddTranslation(new Translation(Convert.ToInt32(translation.ID),
                            translation.Translations[i].ToString()));
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
                new LanguageRepository().AddLanguages(languages);

                Console.WriteLine($"Languages saved to database! ({stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The languages could not be saved to the database.");
                Console.WriteLine(e.Message);
            }

            stopwatch.Stop();

            Console.WriteLine($"Done! ({stopwatch.ElapsedMilliseconds}ms)");
        }
    }
}