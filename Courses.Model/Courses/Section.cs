using System.ComponentModel.DataAnnotations;


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