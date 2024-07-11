using System.Runtime.Serialization;


namespace Courses.Model.Courses.Testings
{
    public class Testing
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public EnumTestingCategory Category { get; set; } = EnumTestingCategory.Basic;

        // Число попыток для выполнения конкретного теста, зависит от категории теста
        public int NumberOfAttempts { get; set; }
        public int? CutScorePercentages { get; set; } // Проходной балл
        public List<Question> Questions { get; set; } = new();
    }

    public enum EnumTestingCategory
    {
        [EnumMember(Value = "Basic")] Basic = 1,
        [EnumMember(Value = "Entrance")] Entrance = 2,
        [EnumMember(Value = "Final")] Final = 3
    }
}