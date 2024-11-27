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
            if (secondChapId == null) // First chapter delete case
            {
                var followingChapters = Chapters.Where(s => s.Id > firstChapId).ToArray();
                foreach (var chapter in followingChapters)
                {
                    chapter.OrderInPart -= 1;
                }

                return;
            }

            // Chapters swap case
            var secondSection = Chapters.First(s => s.Id == (int)secondChapId);
            (firstChapter.OrderInPart, secondSection.OrderInPart) // swap
                = (secondSection.OrderInPart, firstChapter.OrderInPart);
        }

        /// <summary>
        /// Выполняет перемещение части внутри курса
        /// </summary>
        /// <param name="orientation"></param>
        public void Move(EnumMoveOrientation orientation)
        {
            Part secondPart;
            if (orientation == EnumMoveOrientation.Up)
            {
                secondPart = Course.Parts.First(p => p.OrderInCourse == (OrderInCourse - 1));
                Course.ReorderParts(Id, secondPart.Id);
                return;
            }

            secondPart = Course.Parts.First(p => p.OrderInCourse == (OrderInCourse + 1));
            Course.ReorderParts(secondPart.Id, Id);
        }

        /// <summary>
        /// Возвращает true если порядок части в курсе
        /// можно изменить в соответствии с orientation.
        /// False в противном случае
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public bool IsAbleToMove(EnumMoveOrientation orientation)
        {
            if (orientation == EnumMoveOrientation.Up && OrderInCourse == 1)
            {
                return false;
            }

            var isLastPart = Course.Parts.Max(p => p.OrderInCourse) == OrderInCourse;
            if (orientation == EnumMoveOrientation.Down && isLastPart)
            {
                return false;
            }

            return true;
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

        public double GetPrice()
        {
            return Chapters
                .Where(ch => ch.SaleableProductId != null)
                .Sum(ch =>
                ch.SaleableProduct?.PriceInRubles ??
                throw new ArgumentException("Не загружены главы или цены (SaleableProduct)"));
        }
    }
}