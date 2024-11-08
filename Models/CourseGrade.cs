using System.ComponentModel.DataAnnotations;
using AuthSystem.Areas.Identity.Data;

namespace AuthSystem.Models
{
    public class CourseGrade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        public string Grade { get; set; } = "";

        [Required(ErrorMessage = "Course name is required")]
        public string CourseName { get; set; } = "";

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Teacher's name is required")]
        public string TeachersName { get; set; } = "";

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = "";

        public ApplicationUser? User { get; set; }

        // Add TeacherId property
        public string TeacherId { get; set; } = "";  // Add this line
    }


}