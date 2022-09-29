namespace StudentSystem.Data.Models.StudentSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Lesson : BaseModel
    {
        public Lesson()
        {
            this.Resources = new HashSet<Resource>();
        }

        [Required]
        [MaxLength(LessonTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(LessonContentMaxLength)]
        public string Content { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Resource> Resources { get; set; }
    }
}
