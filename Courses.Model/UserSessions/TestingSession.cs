using Courses.Model.Courses.Testings;
using Courses.Model.Users;

namespace Courses.Model.UserSessions
{
    public class TestingSession
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int TestingId { get; set; }
        public Testing Testing { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime DateFinish { get; set; }
        public List<Solution> Solutions { get; set; }
    }
}