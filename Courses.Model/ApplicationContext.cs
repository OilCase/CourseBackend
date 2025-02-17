using Courses.Model.Courses;
using Courses.Model.Courses.Testings;
using Courses.Model.Labels;
using Courses.Model.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Courses.Model
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole, string>
    {
        public static string? EnvironmentName;
        public static string? ConnectionString;

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
            Debug.WriteLine(Database.ProviderName);
            Debug.WriteLine(Database.GetConnectionString());
        }

        public static DbContextOptions<ApplicationContext> GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<ApplicationContext>().UseNpgsql(connectionString).Options;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            InitRelationships(builder);

            PrePopulateDatabase(builder);

            CreateIndexes(builder);

            DefineAutoIncludes(builder);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            RemoveTestingsWithChapters();
            RemoveContentWith<Question>();
            RemoveContentWith<Section>();
            RemoveContentWith<Page>();
            return base.SaveChanges();
        }

        public static void CreateIndexes(ModelBuilder builder)
        {
            builder.Entity<CourseAuthor>()
                .HasIndex(e => new { e.UserId, e.CourseId })
                .IsUnique();

            builder.Entity<Course>()
                .HasIndex(c => c.Title)
                .IsUnique();

            builder.Entity<Label>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }

        public static void PrePopulateDatabase(ModelBuilder builder)
        {
            builder.Entity<Language>().HasData(
                new() { Id = "en", Name = "English" },
                new() { Id = "ru", Name = "Русский" },
                new() { Id = "fr", Name = "Français" }
            );

            builder.Entity<IdentityRole>().HasData(
                Enum.GetNames(typeof(EnumUserRoles)).Select(userRole =>
                    new IdentityRole()
                    {
                        Name = userRole,
                        NormalizedName = userRole.ToUpper(),
                    }).ToArray()
                );
        }

        public static void InitRelationships(ModelBuilder builder)
        {
            builder.Entity<Section>()
                .HasOne(s => s.Chapter)
                .WithMany(c => c.Sections)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Chapter>()
                .HasOne(c => c.Testing)
                .WithOne(t => t.Chapter)
                .HasForeignKey<Testing>(t => t.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LocalizationValue>()
                .HasOne(lv => lv.Language)
                .WithMany()
                .HasForeignKey(lv => lv.LanguageId)
                .IsRequired();

            builder.Entity<Chapter>()
                .HasOne(c => c.Part)
                .WithMany(p => p.Chapters)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Part>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Parts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                .HasMany(c => c.Testings)
                .WithOne(t => t.Course)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Testing>()
                .HasMany(t => t.Questions)
                .WithOne(q => q.Testing)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void DefineAutoIncludes(ModelBuilder builder)
        {
            builder.Entity<Localization>().Navigation(l => l.Values).AutoInclude();
        }

        /// <summary>
        /// Обеспечивает удаление тестов глав
        /// вместе с главами
        /// </summary>
        private void RemoveTestingsWithChapters()
        {
            var testingsForDelete = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity)
                .OfType<Chapter>()
                .Select(c => c.Testing)
                .ToList();

            RemoveRange(testingsForDelete);
        }

        /// <summary>
        /// Обеспечивает автоматическое удаление
        /// контента вместе с родительскими сущностями
        /// ContentOwner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RemoveContentWith<T>() where T : ContentOwner
        {
            var contentForDelete = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity)
                .OfType<T>()
                .Select(s => s.Content)
                .ToList();

            RemoveRange(contentForDelete);
        }

        /// <summary>
        /// Удаляет директорию если она существует
        /// по переданному пути. 
        /// </summary>
        /// <param name="directoryPath"></param>
        private void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<SaleableProduct> SaleableProducts { get; set; }

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Localization> Localizations { get; set; }
        public virtual DbSet<LocalizationValue> LocalizationValues { get; set; }
        public virtual DbSet<Translation> Translations { get; set; }

        public virtual DbSet<Content> Content { get; set; }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Direction> Directions { get; set; }
        public virtual DbSet<CourseDirection> CourseDirections { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<InitialPage> InitialPages { get; set; }
        public virtual DbSet<FinalPage> FinalPages { get; set; }

        public virtual DbSet<CourseAuthor> CourseAuthors { get; set; }

        public virtual DbSet<Testing> Testings { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Competence> Competences { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<CourseChange> CourseChanges { get; set; }

        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Heading> Headings { get; set; }
    }
}
