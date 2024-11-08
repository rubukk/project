using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Areas.Identity.Data
{
    public class Teacher : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
    }
}
