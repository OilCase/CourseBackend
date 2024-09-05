using static System.Collections.Specialized.BitVector32;

namespace Courses.Model.Courses
{
    public class Part
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = "";
        public int OrderInCourse { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

        /// <summary>
        /// Формирует имя части из индекса части и заголовка части
        /// </summary>
        public string GetFullTitle() => $"Часть {OrderInCourse}: {Title}";

        /// <summary>
        /// Вычисляет новые значения порядков
        /// глав в части
        /// </summary>
        /// <param name="firstChapId"></param>
        /// <param name="secondChapId"></param>
        public void ReorderChapters(int firstChapId, int? secondChapId = null)
        {
            var firstChapter = Chapters.First(s => s.Id == firstChapId);
            if (secondChapId == null) // First section delete case
            {
                var followingChapters = Chapters.Where(s => s.Id > firstChapId).ToArray();
                foreach (var chapter in followingChapters)
                {
                    chapter.OrderInPart -= 1;
                }

                return;
            }

            // Sections swap case
            var secondSection = Chapters.First(s => s.Id == (int)secondChapId);
            (firstChapter.OrderInPart, secondSection.OrderInPart) // swap
                = (secondSection.OrderInPart, firstChapter.OrderInPart);
        }

        public void AddChapter()
        {
            var chapter = new Chapter()
            {
                Course = Course,
                CourseId = Course.Id,
                OrderInPart = 1
            };
            if (Course.IsPartialAvailable)
            {
                chapter.SaleableProduct = new()
                {
                    PriceInRubles = Chapter.DefaultPriceInRubles
                };
            }
            chapter.AddSection();
            chapter.AddTesting();
            Chapters.Add(chapter);
        }
    }
}