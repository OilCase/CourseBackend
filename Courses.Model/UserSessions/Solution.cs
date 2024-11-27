using Courses.Model.Courses.Testings;
using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Courses.Model.UserSessions
{
    public class Solution
    {
        public int Id { get; set; }
        public int TestingSessionId { get; set; }
        public TestingSession TestingSession { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }
    }

    //public record SolutionView
    //{
    //    [JsonProperty("id")]
    //    [JsonPropertyName("id")]
    //    public int Id { get; set; }

    //    [JsonProperty("assignment_text")]
    //    [JsonPropertyName("assignment_text")]
    //    public string Text { get; set; }

    //    [JsonProperty("answers")]
    //    [JsonPropertyName("answers")]
    //    public List<AnswerView> Answers { get; set; }
    //}

    //public record TestPageView()
    //{
    //    [JsonProperty("solutions")]
    //    [JsonPropertyName("solutions")]
    //    public List<SolutionView> Solutions { get; set; }

    //    [JsonProperty("current_time")]
    //    [JsonPropertyName("current_time")]
    //    public long CurrentTime { get; set; } // время начала сессии в секундах от начала эпохи

    //    [JsonProperty("session_deadline")]
    //    [JsonPropertyName("session_deadline")]
    //    public long SessionDeadline { get; set; } // время конца сессии в секундах от начала эпохи
    //}
}