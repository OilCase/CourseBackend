using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;


namespace Courses.Model.Courses
{
    public class Section : ContentOwner
    {
        [Key] public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public string? Title { get; set; } = "";
        public int OrderInChapter { get; set; }

        /// <summary>
        /// Выполняет перемещение раздела внутри главы
        /// </summary>
        /// <param name="orientation"></param>
        public void Move(EnumMoveOrientation orientation)
        {
            Section secondSection;
            if (orientation == EnumMoveOrientation.Up)
            {
                secondSection = Chapter.Sections.First(s => s.OrderInChapter == (OrderInChapter - 1));
                Chapter.ReorderSections(Id, secondSection.Id);
                return;
            }

            secondSection = Chapter.Sections.First(s => s.OrderInChapter == (OrderInChapter + 1));
            Chapter.ReorderSections(secondSection.Id, Id);
        }

        /// <summary>
        /// Возвращает true если порядок раздела в главе
        /// можно изменить в соответствии с orientation.
        /// False в противном случае
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public bool IsAbleToMove(EnumMoveOrientation orientation)
        {
            if (orientation == EnumMoveOrientation.Up && OrderInChapter == 1)
            {
                return false;
            }

            var isLastSection = Chapter.Sections.Max(s => s.OrderInChapter) == OrderInChapter;
            if (orientation == EnumMoveOrientation.Down && isLastSection)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Формирует имя раздела из индекса части, индекса главы,
        /// индекса раздела и заголовка раздела
        /// </summary>
        public string GetFullTitle()
        {
            var (partIndex, chapIndex, secIndex) = GetIndexes();
            return $"Раздел {partIndex}.{chapIndex}.{secIndex}: {Title}";
        }

        /// <summary> Возвращает кортеж: индекс родительской части, родительской главы и порядковый номер раздела в главе </summary>
        public (int partIndex, int chapterIndex, int sectionIndex) GetIndexes() =>
               (Chapter.Part.OrderInCourse, Chapter.OrderInPart, OrderInChapter);

        public Section()
        {
        }

        public Section(Course course)
        {
            Course = course;
            Content = new Content(course);
        }
    }
}