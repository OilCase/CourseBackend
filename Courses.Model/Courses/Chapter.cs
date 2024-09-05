using System.Linq;
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
        /// Вычисляет новые значения порядков
        /// разделов в главе
        /// </summary>
        /// <param name="firstSecId"></param>
        /// <param name="secondSecId"></param>
        public void ReorderSections(int firstSecId, int? secondSecId = null)
        {
            var firstSection = Sections.First(s => s.Id == firstSecId);
            if (secondSecId == null) // First section delete case
            {
                var followingSections = Sections.Where(s => s.Id > firstSecId).ToArray();
                foreach (var section in followingSections)
                {
                    section.OrderInChapter -= 1;
                }

                return;
            }

            // Sections swap case
            var secondSection = Sections.First(s => s.Id == (int)secondSecId);
            (firstSection.OrderInChapter, secondSection.OrderInChapter) // swap
                = (secondSection.OrderInChapter, firstSection.OrderInChapter);
        }

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
            Sections.Add(new Section(Course)
            {
                OrderInChapter = 1
            });
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