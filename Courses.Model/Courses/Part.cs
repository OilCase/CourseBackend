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

        public void AddChapter()
        {
            var chapter = new Chapter()
            {
                Course = Course,
                CourseId = Course.Id,
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