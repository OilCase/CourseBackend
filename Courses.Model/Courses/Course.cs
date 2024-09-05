using System.Runtime.Serialization;

using Courses.Model.Courses.Testings;


namespace Courses.Model.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public bool IsPartialAvailable { get; set; }

        public bool IsFree { get; set; }

        public int? SaleableProductId { get; set; }
        public SaleableProduct? SaleableProduct { get; set; }

        public string LanguageId { get; set; }
        public Language Language { get; set; }

        public InitialPage? InitialPage { get; set; }
        public FinalPage FinalPage { get; set; }

        public EnumCourseFormat CourseFormat { get; set; }
        public EnumCourseType CourseType { get; set; }
        public EnumCourseStatus Status { get; set; } = EnumCourseStatus.InDevelopment;

        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Authors { get; set; }
        public string? TargetAudience { get; set; }
        public string? EducationMethods { get; set; }
        public string? EducationResults { get; set; }

        public DateTime? SalesTerminationDate { get; set; }

        public int? DurationAcademicHours { get; set; }

        // Длительность курса в рабочих днях, относится только к синхронным курсам
        public int? DurationWorkDays { get; set; }

        // Дата старта существует только для синхронных курсов
        public DateTime? DateStart { get; set; }

        // Дата завершения существует только для синхронных курсов
        public DateTime? DateFinish { get; set; }

        public virtual ICollection<CourseAuthor> CourseAuthors { get; set; } = new List<CourseAuthor>();
        public virtual ICollection<CourseDirection> CourseDirections { get; set; } = new List<CourseDirection>();
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
        public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
        public virtual ICollection<Testing> Testings { get; set; } = new List<Testing>();
        public virtual ICollection<CourseChange> CourseChanges { get; set; } = new List<CourseChange>();

        /// <summary>
        /// Возвращает true если основные настройки заполнены
        /// и false в противном случае
        /// </summary>
        public bool SettingsFilled()
        {
            if (!CourseAuthors.Any()
                || !CourseDirections.Any()
                || SalesTerminationDate == null
                || DurationAcademicHours == null)
            {
                return false;
            }

            switch (CourseType, CourseFormat, IsFree, IsPartialAvailable)
            {
                case (EnumCourseType.Synchronous, EnumCourseFormat.Online, false, false):
                    return SaleableProduct != null
                           && DurationWorkDays != null
                           && DateStart != null
                           && DateFinish != null;

                case (EnumCourseType.Asynchronous, EnumCourseFormat.Online, false, false):
                    return SaleableProduct != null;

                case (EnumCourseType.Synchronous, EnumCourseFormat.Online, true, false):
                    return DurationWorkDays != null
                           && DateStart != null
                           && DateFinish != null;
            }

            return true;
        }

        /// <summary>
        /// Возвращает true если все информационные поля
        /// курса заполнены
        /// </summary>
        public bool InformationFilled()
        {
            if (string.IsNullOrEmpty(Title)
             || string.IsNullOrEmpty(Authors)
             || string.IsNullOrEmpty(Description)
             || string.IsNullOrEmpty(TargetAudience)
             || string.IsNullOrEmpty(EducationMethods)
             || string.IsNullOrEmpty(EducationResults))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет изменяемые параметры курса
        /// на соответствие бизнес-логике
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        public void ValidateSettings()
        {
            var exceptions = new List<Exception>();
            if (CourseType == EnumCourseType.Synchronous)
            {
                ValidateSync(exceptions);
            }

            if (CourseType == EnumCourseType.Asynchronous)
            {
                ValidateAsync(exceptions);
            }

            if (!IsFree && !IsPartialAvailable && SaleableProduct.PriceInRubles == null)
            {
                exceptions.Add(new ArgumentException("Стоимость платного полного курса должна быть задана"));
            }

            if (IsFree && SaleableProduct != null)
            {
                exceptions.Add(new ArgumentException("Нельзя задать стоимость для бесплатного курса"));
            }

            if (IsPartialAvailable && SaleableProduct != null)
            {
                exceptions.Add(new ArgumentException("Стоимость частичного курса определяется по стоимости его содержимого"));
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Проверяет поля для синхронного курса 
        /// на соответствие бизнес-логике,
        /// все несоответствия формирует в 
        /// AggregateException
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        protected void ValidateSync(List<Exception> exceptions)
        {
            if (DurationWorkDays == null)
            {
                exceptions.Add(new ArgumentNullException($"Значение свойства {nameof(DurationWorkDays)} для синхронного курса должно быть задано"));
            }

            if (DateStart == null)
            {
                exceptions.Add(new ArgumentNullException($"Значение свойства {nameof(DateStart)} для синхронного курса должно быть задано"));
            }

            if (DateFinish == null)
            {
                exceptions.Add(new ArgumentNullException($"Значение свойства {nameof(DateFinish)} для синхронного курса должно быть задано"));
            }

            if (DateFinish <= DateStart)
            {
                exceptions.Add(new ArgumentException("Дата конца не может быть раньше даты начала"));
            }

            if (SalesTerminationDate >= DateStart)
            {
                exceptions.Add(new ArgumentException("Курс должен быть снят с продажи до начала занятий"));
            }
        }

        /// <summary>
        /// Проверяет поля для асинхронного курса 
        /// на соответствие бизнес-логике,
        /// все несоответствия формирует в 
        /// AggregateException
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        protected void ValidateAsync(List<Exception> exceptions)
        {
            if (DurationWorkDays != null)
            {
                exceptions.Add(new ArgumentException($"Значение свойства {nameof(DurationWorkDays)} для асинхронного курса не может быть задано"));
            }

            if (DateStart != null)
            {
                exceptions.Add(new ArgumentException($"Значение свойства {nameof(DateStart)} для асинхронного курса не может быть задано"));
            }

            if (DateFinish != null)
            {
                exceptions.Add(new ArgumentException($"Значение свойства {nameof(DateFinish)} для асинхронного курса не может быть задано"));
            }
        }

        public void AddEntranceTest()
        {
            var testing = new Testing()
            {
                Title = "Entrance Test 1",
                Category = EnumTestingCategory.Entrance,
                NumberOfAttempts = 1,
            };

            var question = new Question()
            {
                Content = new Content()
                {
                    Course = this
                }
            };
            question.Answers.AddRange(new List<Answer>
            {
                new Answer(),
                new Answer(),
                new Answer(),
                new Answer()
            });

            testing.Questions.Add(question);
            Testings.Add(testing);
        }

        public void AddFinalTest()
        {
            var testing = new Testing()
            {
                Title = "Final Test 1",
                Category = EnumTestingCategory.Final,
                NumberOfAttempts = 1000
            };
            var question = new Question()
            {
                Content = new()
                {
                    Course = this
                }
            };
            question.Answers.AddRange(new List<Answer>
            {
                new Answer(),
                new Answer(),
                new Answer(),
                new Answer()
            });

            testing.Questions.Add(question);
            Testings.Add(testing);
        }

        /// <summary>
        /// Добавляет к курсу минимальный набор
        /// компонентов:
        /// 1 часть, 1 глава, 1 раздел, 1 тест в главе,
        /// 1 входной тест, 1 итоговый тест,
        /// 1 вводная страница и 1 страница итогов
        /// </summary>
        public void AddComponents()
        {
            bool isSaleable = !IsFree && !IsPartialAvailable;
            if (isSaleable)
            {
                SaleableProduct = new SaleableProduct()
                {
                    PriceInRubles = 0.0,
                };
            }
            
            Section section = new Section(this)
            {
                OrderInChapter = 1
            };

            Chapter chapter = new Chapter()
            {
                Course = this,
                OrderInPart = 1
            };
            if (IsPartialAvailable)
            {
                chapter.SaleableProduct = new()
                {
                    PriceInRubles = Chapter.DefaultPriceInRubles
                };
            }
            chapter.AddTesting(this);
            chapter.Sections.Add(section);

            Part part = new Part()
            {
                OrderInCourse = 1
            };
            part.Chapters.Add(chapter);

            Parts.Add(part);
            AddEntranceTest();
            AddFinalTest();

            InitialPage = new InitialPage()
            {
                Course = this,
            };
            InitialPage.Content.Course = this;

            FinalPage = new FinalPage()
            {
                Course = this,
            };
            FinalPage.Content.Course = this;
        }
    }

    public enum EnumCourseFormat
    {
        [EnumMember(Value = "Offline")] Offline = 1,
        [EnumMember(Value = "Online")] Online = 2
    }

    public enum EnumCourseType
    {
        [EnumMember(Value = "Synchronous")] Synchronous = 1,
        [EnumMember(Value = "Asynchronous")] Asynchronous = 2
    }

    public enum EnumCourseStatus
    {
        [EnumMember(Value = "InDevelopment")] InDevelopment = 1, // В разработке
        [EnumMember(Value = "OnModeration")] OnModeration = 2, // На модерации
        [EnumMember(Value = "Published")] Published = 3,       // Опубликован 
        [EnumMember(Value = "Withdrawn")] Withdrawn = 4,       // Снят с витрины (регулируется job'ом в бд)
        [EnumMember(Value = "Archived")] Archived = 5,         // Архивирован (регулируется job'ом в бд)
    }
}