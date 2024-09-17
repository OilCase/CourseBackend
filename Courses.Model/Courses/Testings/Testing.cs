using System.IO;
using System.Runtime.Serialization;


namespace Courses.Model.Courses.Testings
{
    public class Testing
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public EnumTestingCategory Category { get; set; } = EnumTestingCategory.Basic;

        // Число попыток для выполнения конкретного теста, зависит от категории теста
        public int NumberOfAttempts { get; set; }
        public int? CutScorePercentages { get; set; } // Проходной балл
        public int? ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
        public List<Question> Questions { get; set; } = new();

        public string GetFullTitle()
        {
            if (Category == EnumTestingCategory.Basic)
            {
                var (partIndex, chapIndex) = Chapter!.GetIndexes();
                return $"Тест {partIndex}.{chapIndex}: {Title}";
            }

            return $"{Title}";
        }

        /// <summary>
        /// Вычисляет новые значения порядков
        /// частей в курсе
        /// </summary>
        /// <param name="firstQuestionId"></param>
        /// <param name="secondQuestionId"></param>
        public void ReorderQuestions(int firstQuestionId, int? secondQuestionId = null)
        {
            var firstQuestion = Questions.First(q => q.Id == firstQuestionId);
            if (secondQuestionId == null) // First part delete case
            {
                var followingQuestions = Questions.Where(p => p.Id > firstQuestionId).ToArray();
                foreach (var question in followingQuestions)
                {
                    question.OrderInTesting -= 1;
                }

                return;
            }

            // Part swap case
            var secondQuestion = Questions.First(q => q.Id == (int)secondQuestionId);
            (firstQuestion.OrderInTesting, secondQuestion.OrderInTesting) // swap
                = (secondQuestion.OrderInTesting, firstQuestion.OrderInTesting);
        }
    }

    public enum EnumTestingCategory
    {
        [EnumMember(Value = "Basic")] Basic = 1,
        [EnumMember(Value = "Entrance")] Entrance = 2,
        [EnumMember(Value = "Final")] Final = 3
    }
}