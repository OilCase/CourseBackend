﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Courses.Model
{
    public class Language
    {
        [Key]
        public string Id { get; set; } // короткий код языка, например en-us 
        public string Name { get; set; } // полное название 
    }

    public class Localization
    {
        [Key] 
        public int Id { get; set; }
        public List<LocalizationValue> Values { get; set; }

        public string? this[string languageId]
        {
            get => Values.FirstOrDefault(v => v.LanguageId == languageId)?.Value;
            set => Values.FirstOrDefault(v => v.LanguageId == languageId)!.Value = value!;
        }

        /// <summary>
        /// Добавляет LocalizationValues к 
        /// существующей Localization
        /// </summary>
        /// <param name="locValues"></param>
        public void AddValues(Dictionary<string, string> locValues)
        {
            foreach (var locValue in locValues)
            {
                Values.Add(new()
                {
                    Localization = this,
                    LanguageId = locValue.Key,
                    Value = locValue.Value
                });
            }
        }

        /// <summary>
        /// Возвращает словарь 
        /// код языка : локализация,
        /// полученный из Values
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            foreach(var value in Values)
            {
                dict[value.LanguageId] = value.Value;
            }

            return dict;
        }
    }

    /// <summary>
    /// Переводы бэковских сущностей
    /// </summary>
    public class Translation
    {
        [Key]
        public int Id { get; set; }
        public string LanguageId { get; set; }
        public Language Language { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class LocalizationValue
    {
        [Key] 
        public int Id { get; set; }

        public string Value { get; set; }

        public int LocalizationId { get; set; }
        public Localization? Localization { get; set; }

        [ForeignKey(nameof(Language))] 
        public string LanguageId { get; set; }
        public Language? Language { get; set; }
    }
}