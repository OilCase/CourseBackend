﻿using System.Linq;
using System.Security.AccessControl;
using Courses.Model.Courses.Testings;

namespace Courses.Model.Courses
{


    public class Chapter
    {
        public const double DefaultPriceInRubles = 1000.0;

        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; } = null!;
        public string? Title { get; set; } = "";
        public int? SaleableProductId { get; set; }
        public SaleableProduct? SaleableProduct { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int OrderInPart { get; set; }

        public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

        public Testing Testing { get; set; }

        /// <summary>
        /// Выполняет перемещение главы внутри части
        /// </summary>
        /// <param name="orientation"></param>
        public void Move(EnumMoveOrientation orientation)
        {
            Chapter secondChapter;
            if (orientation == EnumMoveOrientation.Up)
            {
                secondChapter = Part.Chapters.First(c => c.OrderInPart == (OrderInPart - 1));
                Part.ReorderChapters(Id, secondChapter.Id);
                return;
            }

            secondChapter = Part.Chapters.First(c => c.OrderInPart == (OrderInPart + 1));
            Part.ReorderChapters(secondChapter.Id, Id);
        }

        /// <summary>
        /// Возвращает true если порядок главы в части
        /// можно изменить в соответствии с orientation.
        /// False в противном случае
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public bool IsAbleToMove(EnumMoveOrientation orientation)
        {
            if (orientation == EnumMoveOrientation.Up && OrderInPart == 1)
            {
                return false;
            }

            var isLastChapter = Part.Chapters.Max(c => c.OrderInPart) == OrderInPart;
            if (orientation == EnumMoveOrientation.Down && isLastChapter)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Вычисляет новые значения порядков
        /// разделов в главе
        /// </summary>
        /// <param name="firstSecId"></param>
        /// <param name="secondSecId"></param>
        public void ReorderSections(int firstSecId, int? secondSecId = null)
        {
            var firstSection = Sections.First(s => s.Id == firstSecId);
            if (secondSecId == null) // First section delete case
            {
                var followingSections = Sections.Where(s => s.Id > firstSecId).ToArray();
                foreach (var section in followingSections)
                {
                    section.OrderInChapter -= 1;
                }

                return;
            }

            // Sections swap case
            var secondSection = Sections.First(s => s.Id == (int)secondSecId);
            (firstSection.OrderInChapter, secondSection.OrderInChapter) // swap
                = (secondSection.OrderInChapter, firstSection.OrderInChapter);
        }

        /// <summary>
        ///  Формирует имя главы из индекса части, индекса главы и заголовка главы
        /// </summary>
        public string GetFullTitle()
        {
            var (partIndex, chapIndex) = GetIndexes();
            return $"Глава {partIndex}.{chapIndex}: {Title}";
        }

        /// <summary> Возвращает кортеж: индекс родительской части и порядковый номер главы в части </summary>
        public (int partIndex, int chapterIndex) GetIndexes() => (Part.OrderInCourse, OrderInPart); 

        public void AddSection()
        {
            Sections.Add(new Section(Course)
            {
                OrderInChapter = 1
            });
        }

        /// <summary>
        /// Добавляет к главе тест,
        /// содержащий один обязательный
        /// пустой вопрос
        /// </summary>
        public void AddTesting(Course? course = null)
        {
            var testing = new Testing()
            {
                Course = this.Course,
                NumberOfAttempts = 1000,
            };
            testing.AddQuestions();
            Testing = testing;
            Course.Testings.Add(testing);
        }

        public bool IsReadyForDelete()
        {
            var part = Part;
            return part.Chapters.Count >= 2;
        }
    }
}