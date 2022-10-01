namespace StudentSystem.Data.Models.StudentSystem
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.UserCourses = new HashSet<UserCourse>();
            this.Reviews = new HashSet<Review>();
        }

        public ICollection<UserCourse> UserCourses { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
