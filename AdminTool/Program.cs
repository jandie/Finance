using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FinanceLibrary.Classes.Language;
using FinanceLibrary.Logic;
using Newtonsoft.Json.Linq;

namespace AdminTool
{
    internal class Program
    {
        private static Stopwatch _stopwatch;

        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Insert command:");
                ExecuteCommand(Console.ReadLine());
                Console.WriteLine("");
            }
        }

        private static void ExecuteCommand(string command)
        {
            switch (command.Split(' ')[0])
            {
                case "language":
                    UpdateLanguage();
                    break;
                case "remove":
                    ExecuteRemove(command);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        private static void ExecuteRemove(string command)
        {
            if (command.Split(' ').Length < 3)
            {
                Console.WriteLine("No valid remove option found");
                return;
            }

            switch (command.Split(' ')[1])
            {
                case "user":
                    RemoveUser(command.Split(' ')[2]);
                    break;
                default:
                    Console.WriteLine("No valid remove option found");
                    break;
            }
        }

        private static void RemoveUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            Console.WriteLine($"Removing user with email {email} ...");

            new DeleteLogic().DeleteUser(email);

            _stopwatch.Stop();

            Console.WriteLine($"User deleted! ({_stopwatch.ElapsedMilliseconds}ms)");
        }

        private static void UpdateLanguage()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            List<Language> languages = ParseLanguages(GetLanguageJson());
            PersistLanguages(languages);

            _stopwatch.Stop();

            Console.WriteLine($"Done! ({_stopwatch.ElapsedMilliseconds}ms)");
        }

        private static string GetLanguageJson()
        {
            string json = "";
            string path = Environment.GetEnvironmentVariable("FINANCE_LANGUAGE_PATH",
                EnvironmentVariableTarget.Machine);

            if (path == null) return null;

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

                Console.WriteLine($"File read! ({_stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return json;
        }

        private static List<Language> ParseLanguages(string json)
        {
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
                    for (int i = 0; i < translation.Translations.Count; i++)
                    {
                        languages[i].AddTranslation(new Translation(Convert.ToInt32(translation.ID),
                            translation.Translations[i].ToString()));
                    }
                }

                Console.WriteLine($"Json file parsed! ({_stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be converted into library:");
                Console.WriteLine(e.Message);
            }

            return languages;
        }

        private static void PersistLanguages(List<Language> languages)
        {
            try
            {
                new LanguageLogic().AddLanguages(languages);

                Console.WriteLine($"Languages saved to database! ({_stopwatch.ElapsedMilliseconds}ms)");
            }
            catch (Exception e)
            {
                Console.WriteLine("The languages could not be saved to the database.");
                Console.WriteLine(e.Message);
            }
        }
    }
}