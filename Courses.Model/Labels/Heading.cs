using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Courses.Model.Labels
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Description), IsUnique = true)]
    public class Heading // Разделы меток
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1)]
        public string Name { get; set; }

        [Required]
        [StringLength(1)]
        public string Description { get; set; }

        public List<Label> Labels { get; set; } = new List<Label>();
    }
}
