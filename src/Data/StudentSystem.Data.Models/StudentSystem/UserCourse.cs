namespace StudentSystem.Data.Models.StudentSystem
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    public class UserCourse : BaseModel
    {
        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
