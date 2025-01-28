using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Courses.Model.Courses
{
    /// <summary>
    /// Родительский класс, обобщающий
    /// сущности, обладающие контентом
    /// (вопросы, страницы, разделы)
    /// </summary>
    public class ContentOwner
    {
        public int ContentId { get; set; }
        public Content Content { get; set; } = new();
    }

    /// <summary>
    /// Представляет содержимое раздела, теста, страницы курса.
    /// Каждой сущности контента соответствует директория.
    /// При удалении сущности контента удаляется и его директория
    /// (модифицирован SaveChanges в ApplicationContext)
    /// </summary>
    public class Content
    {
        [Key] public int Id { get; set; }
        public string? Text { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        /// <summary> Идентификатор файла для файлового хранилища </summary>
        [NotMapped] public string FileId => GetFileId();

        /// <summary> Формирует id файла из id курса и id контента </summary>
        private string GetFileId() => $"course-{CourseId}/{Id}";

        /// <summary/>
        public byte[] GetBytesFromString(string input)
        {
            var encoding = Encoding.UTF8;

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input string cannot be null.");
            }

            return encoding.GetBytes(input);
        }


        public Content() { }

        public Content(Course course)
        {
            Course = course;
        }
    }
}