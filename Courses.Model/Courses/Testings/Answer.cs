namespace Courses.Model.Courses.Testings
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}