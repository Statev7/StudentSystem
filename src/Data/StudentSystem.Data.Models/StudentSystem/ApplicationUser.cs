namespace StudentSystem.Data.Models.StudentSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using static Data.Common.Constants;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.UserCourses = new HashSet<UserCourse>();
            this.Reviews = new HashSet<Review>();
            this.UserRoles = new HashSet<ApplicationUserRole>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        public string ImageURL { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

        public ICollection<UserCourse> UserCourses { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
