using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;

namespace Courses.Model.Courses.Testings
{
    public class Question: ContentOwner
    {
        [Key]
        public int Id { get; set; }
        public int TestingId { get; set; }
        public string Title { get; set; } = "";
        public Testing Testing { get; set; }
        public int? DirectionId { get; set; }
        public Direction Direction { get; set; }
        public string? CompetenceId { get; set; }
        public Competence? Competence { get; set; }
        public int OrderInTesting { get; set; }
        public bool ShowFullTitle { get; set; } // отображать тело вопроса в названии или нет

        // Вес вопроса, определяется в админке и используется в OilID
        public int CorrectAnswerScore { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public Question() { }


        /// <summary>
        /// Формирует название вопроса из порядкового номера вопроса в тесте
        /// и текста содержания вопроса (если выставлен флажок ShowFullTitle)
        /// </summary>
        public string GetFullTitle()
        {
            if (ShowFullTitle)
            {
                return $"Вопрос {OrderInTesting}: {Title}";
            }

            return $"Вопрос {OrderInTesting}";
        }

        /// <summary>
        /// Парсит содержимое вопроса чтобы извлечь первые 90 символов.
        /// Удаляет вёрстку, оставляя только чистый текст
        /// </summary>
        public string GetTitleFromContent(string? content)
        {
            if (content == null)
            {
                return "";
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            string innerText = doc.DocumentNode.InnerText;
            string resultString = HtmlEntity.DeEntitize(innerText).Trim();

            return resultString.Length > 90 ? resultString.Substring(0, 90) : resultString;
        }

        /// <summary>
        /// Добавляет вопросу 4 пустых ответа
        /// </summary>
        public void AddAnswers()
        {
            var answers = new List<Answer>()
            {
                new Answer(),
                new Answer(),
                new Answer(),
                new Answer()
            };
            Answers.AddRange(answers);
        }

        /// <summary>
        /// Проверяет, что вопрос доступен для удаления
        /// (в тесте есть еще вопросы)
        /// </summary>
        /// <returns></returns>
        public bool IsReadyForDelete()
        {
            var questions = Testing.Questions;

            return questions.Count > 1;
        }
    }

    // Компетенции, которые относятся к вопросу
    // Будут использоваться в OilID 
    // для построения спайдерчартов
    public class Competence
    {
        public string Id { get; set; } // Drilling - D
        public string Name { get; set; } // Drilling
    }
}