using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

using Courses.Model.UserSessions;
using Courses.Model.Users;
using Courses.Model.Courses;
using Courses.Model.Courses.Testings;
using Courses.Model.Labels;

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
            DeleteRemovedContentDirectories();
            return base.SaveChanges();
        }

        public static void CreateIndexes(ModelBuilder builder)
        {
            builder.Entity<CourseAuthor>()
                .HasIndex(e => new { e.UserId, e.CourseId})
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

            var shelfLocalization = new Localization() { Id = 1};
            builder.Entity<Localization>().HasData(shelfLocalization);
            var shelfLocalizationValues = new[]
            {
                new LocalizationValue() {Id = 1, LocalizationId = 1, LanguageId = "en", Value = "shelf" },
                new LocalizationValue() {Id = 2, LocalizationId = 1, LanguageId = "ru", Value = "шельф" }
            };
            builder.Entity<LocalizationValue>().HasData(shelfLocalizationValues);

            var geologyLocalization = new Localization() { Id = 2 };
            builder.Entity<Localization>().HasData(geologyLocalization);
            var geologyLocalizationValues = new[]
            {
                new LocalizationValue() {Id = 3, LocalizationId = 2, LanguageId = "en", Value = "geology" },
                new LocalizationValue() {Id = 4, LocalizationId = 2, LanguageId = "ru", Value = "геология" }
            };
            builder.Entity<LocalizationValue>().HasData(geologyLocalizationValues);

            var drillingLocalization = new Localization() { Id = 3 };
            builder.Entity<Localization>().HasData(drillingLocalization);
            var drillingLocalizationValues = new[]
            {
                new LocalizationValue() {Id = 5, LocalizationId = 3, LanguageId = "en", Value = "drilling" },
                new LocalizationValue() {Id = 6, LocalizationId = 3, LanguageId = "ru", Value = "бурение" }
            };
            builder.Entity<LocalizationValue>().HasData(drillingLocalizationValues);

            var shelfDescription = new Localization() { Id = 4 };
            builder.Entity<Localization>().HasData(shelfDescription);
            var shelfDescriptionValues = new[]
            {
                new LocalizationValue() {Id = 7, LocalizationId = 4, LanguageId = "en", Value = "Shelf Description" },
                new LocalizationValue() {Id = 8, LocalizationId = 4, LanguageId = "ru", Value = "шельфовое описание" }
            };
            builder.Entity<LocalizationValue>().HasData(shelfDescriptionValues);

            var geologyDescription = new Localization() { Id = 5 };
            builder.Entity<Localization>().HasData(geologyDescription);
            var geologyDescriptionValues = new[]
            {
                new LocalizationValue() {Id = 9, LocalizationId = 5, LanguageId = "en", Value = "Geology Description" },
                new LocalizationValue() {Id = 10, LocalizationId = 5, LanguageId = "ru", Value = "геологичное описание" }
            };
            builder.Entity<LocalizationValue>().HasData(geologyDescriptionValues);

            var drillingDescription = new Localization() { Id = 6 };
            builder.Entity<Localization>().HasData(drillingDescription);
            var drillingDescriptionValues = new[]
            {
                new LocalizationValue() {Id = 11, LocalizationId = 6, LanguageId = "en", Value = "Drilling Description" },
                new LocalizationValue() {Id = 12, LocalizationId = 6, LanguageId = "ru", Value = "бурительное описание" }
            };
            builder.Entity<LocalizationValue>().HasData(drillingDescriptionValues);

            builder.Entity<Direction>().HasData(
                new()
                {
                    Id = 1,
                    LocalizationId = 1,
                    DescriptionId = 4,
                    IsVisible = true,
                },
                new()
                {
                    Id = 2,
                    LocalizationId = 2,
                    DescriptionId = 5,
                    IsVisible = true,
                },
                new()
                {
                    Id = 3,
                    DescriptionId = 6,
                    LocalizationId = 3,
                    IsVisible = true,
                });
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
        private void RemoveContentWith<T>() where T: ContentOwner
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
        /// Обеспечивает удаление директорий
        /// содержимого на сервере вместе с
        /// удалением сущности содержимого из бд
        /// </summary>
        private void DeleteRemovedContentDirectories()
        {
            var deletedContentList = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity)
                .OfType<Content>();

            foreach (var deletedContent in deletedContentList)
            {
                var path = deletedContent.GetContentDirectoryPath();
                DeleteDirectory(path);
            }
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
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<TestingSession> TestingSessions { get; set; }
        public virtual DbSet<CourseChange> CourseChanges { get; set; }

        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Heading> Headings{ get; set; }
    }
}