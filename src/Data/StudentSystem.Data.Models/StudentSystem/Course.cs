namespace StudentSystem.Data.Models.StudentSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Course : BaseModel
    {
        public Course()
        {
            this.Lessons = new HashSet<Lesson>();
            this.UserCourses = new HashSet<UserCourse>();
        }

        [Required]
        [MaxLength(COURSE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MaxLength(COURSE_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<Lesson> Lessons { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<UserCourse> UserCourses { get; set; }
    }
}
