namespace Courses.Model.Courses
{
    public class Direction
    {
        public int Id { get; set; }
        public int LocalizationId { get; set; }
        public Localization Localization { get; set; }
        public int DescriptionId { get; set; }
        public Localization Description { get; set; }
        public bool IsVisible { get; set; } // видимость направления на сайте
        public DateTime LastChangeDateTime { get; set; }

        public string GetDirectionName(string language)
        {
            return Localization[language] ?? throw new ArgumentException("Не загружена локализация");
        }
    }

    public class CourseDirection
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public int DirectionId { get; set; }
        public Direction Direction { get; set; }
    }
}