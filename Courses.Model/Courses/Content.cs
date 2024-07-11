using System.ComponentModel.DataAnnotations;

namespace Courses.Model.Courses
{
    /// <summary>
    /// Родителский класс, обобщающий
    /// сущности, обладающие контентом
    /// (вопросы, страницы, разделы)
    /// </summary>
    public class ContentOwner
    {
        public int ContentId { get; set; }
        public Content Content { get; set; } = new Content();
    }

    /// <summary>
    /// Представляет содержимое раздела, теста, страницы курса.
    /// Каждой сущности контента соответствует директория.
    /// При удалении сущности контента удаляется и его директория
    /// (модифицирован SaveChanges в ApplicationContext)
    /// </summary>
    public class Content
    {
        [Key]
        public int Id { get; set; }
        public string? Text { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Content()
        {
        }

        public Content(Course course)
        {
            Course = course;
        }

        public string GetContentDirectoryPath() => $"{ContentRootPath}\\courses\\{CourseId}\\{Id}";

        private static string? _ContentRootPath { get; set; }

        public static string? ContentRootPath
        {
            get => _ContentRootPath;
            set
            {
                if (_ContentRootPath == null)
                {
                    _ContentRootPath = value;
                }
                else
                {
                    throw new InvalidOperationException("Уже определён");
                }
            }
        }
    }
}