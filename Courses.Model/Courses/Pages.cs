using System.Runtime.Serialization;

namespace Courses.Model.Courses
{
    public enum EnumPageCategory
    {
        [EnumMember(Value = "Initial")] Initial = 1,
        [EnumMember(Value = "Final")] Final = 2
    }

    public class InitialPage: Page
    {
        public const string Title = "InitialPage";
    }

    public class FinalPage: Page
    {
        public const string Title = "FinalPage";
    }

    public class Page : ContentOwner
    {
        public Page(): base(){}
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}