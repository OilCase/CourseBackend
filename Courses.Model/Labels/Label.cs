using System.ComponentModel.DataAnnotations;

namespace Courses.Model.Labels
{
    public class Label // Метки
    {
        [Key] 
        public int Id { get; set; }

        public string Name { get; set; }
        public int HeadingId { get; set; }
        public Heading Heading { get; set; }
        public int LocalizationId { get; set; }
        public Localization Localization { get; set; }
    }
}