using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Courses.Model.Courses
{
    public class Section: ContentOwner
    {
        [Key] public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public string? Title { get; set; }
        public int OrderInChapter { get; set; }

        /// <summary> Возвращает кортеж: индекс родительской части, родительской главы и порядковый номер раздела в главе </summary>
        public (int partIndex, int chapterIndex, int sectionIndex) GetIndexes() => (Chapter.Part.OrderInCourse, Chapter.OrderInPart, OrderInChapter);

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