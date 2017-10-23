﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FinanceLibrary.Classes.Language;
using FinanceLibrary.Logic;
using Newtonsoft.Json.Linq;

namespace Language_import
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
            if (command.Split(' ').Length < 3) return;

            switch (command.Split(' ')[1])
            {
                case "user":
                    RemoveUser(command.Split(' ')[2]);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        private static void RemoveUser(string email)
        {
            Console.WriteLine($"Removing user with email {email} ...");
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

                Console.WriteLine($"Json file converted! ({_stopwatch.ElapsedMilliseconds}ms)");
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