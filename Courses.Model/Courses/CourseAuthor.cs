using Courses.Model.Users;

namespace Courses.Model.Courses
{
    public class CourseAuthor
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}