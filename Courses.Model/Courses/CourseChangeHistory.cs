namespace Courses.Model.Courses
{
    public class CourseChange
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime ChangeDateTime { get; set; }

    }
}