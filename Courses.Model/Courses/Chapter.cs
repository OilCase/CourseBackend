using System.Security.AccessControl;
using Courses.Model.Courses.Testings;

namespace Courses.Model.Courses
{


    public class Chapter
    {
        public const double DefaultPriceInRubles = 1000.0;

        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; } = null!;
        public string? Title { get; set; } = "";
        public int? SaleableProductId { get; set; }
        public SaleableProduct? SaleableProduct { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int OrderInPart { get; set; }

        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

        public Testing Testing { get; set; }

        /// <summary>
        ///  Формирует имя главы из индекса части, индекса главы и заголовка главы
        /// </summary>
        public string GetFullTitle()
        {
            var (partIndex, chapIndex) = GetIndexes();
            return $"Глава {partIndex}.{chapIndex}: {Title}";
        }

        /// <summary> Возвращает кортеж: индекс родительской части и порядковый номер главы в части </summary>
        public (int partIndex, int chapterIndex) GetIndexes() => (Part.OrderInCourse, OrderInPart); 

        public void AddSection()
        {
            Sections.Add(new Section(Course));
        }

        /// <summary>
        /// Добавляет к главе тест,
        /// содержащий один обязательный
        /// пустой вопрос
        /// </summary>
        public void AddTesting(Course? course = null)
        {
            var testing = new Testing()
            {
                Title = "Testing 1",
                NumberOfAttempts = 1000
                
            };

            var question = new Question();
            if (course == null)
            {
                question.Content = new Content()
                {
                    CourseId = CourseId
                };
            }
            else
            {
                question.Content = new Content()
                {
                    Course = course
                };
            }          
            question.Answers.AddRange(new List<Answer>
            {
                new Answer(),
                new Answer(),
                new Answer(),
                new Answer(),
            });

            testing.Questions.Add(question);
            Testing = testing;
            Course.Testings.Add(testing);
        }

        public bool IsReadyForDelete()
        {
            var part = Part;
            return part.Chapters.Count >= 2;
        }
    }
}