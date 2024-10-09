using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Courses.Model.Localization;


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
        /// Возвращает словарь локализованных имен направления
        /// id языка: значение
        /// </summary>
        public Dictionary<string, string> ToDict() => this.Values
            .Select(v => new { v.LanguageId, v.Value })
            .ToDictionary(x => x.LanguageId, x => x.Value);
        
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

    public static class LocalizationExtensions
    {
        /// <summary>
        /// Проверяет, есть ли среди имеющихся локализаций
        /// пересечения с переданными newLocalizations.
        /// Если пересечения найдены - заполняет поле Message
        /// строкой, аггрегирующей дубликаты
        /// </summary>
        public static LocalizationsValidationResult HaveDuplicates(this IEnumerable<Localization> existingLocalizations, IEnumerable<string> newLocalizations)
        {
            var result = new LocalizationsValidationResult
            {
                Valid = true,
            };

            var existingValues = existingLocalizations.SelectMany(l => l.Values.Select(v => v.Value)).ToArray();
            var intersect = newLocalizations.Intersect(existingValues).ToArray();
            if (intersect.Any())
            {
                result.Valid = false;
                result.Message = string.Join(", ", intersect);
            }

            return result;
        }
    }

    public class LocalizationsValidationResult
    {
        public bool Valid { get; set; }
        public string Message { get; set; }
    };
}