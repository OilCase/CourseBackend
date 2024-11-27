using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace Courses.Model.Users
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum EnumUserRoles
    {
        [EnumMember(Value = "Admin")] Admin = 1,
        [EnumMember(Value = "Author")] Author = 2,
        [EnumMember(Value = "Regular")] Regular = 3
    }
}