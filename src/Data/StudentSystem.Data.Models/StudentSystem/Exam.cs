namespace StudentSystem.Data.Models.StudentSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Exam : BaseModel<int>, IDeletableEntity
    {
        public Exam()
        {
            this.Resources = new HashSet<Resource>();
        }

        [Required]
        [MaxLength(ExamNameMaxLength)]
        public string Name { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public double Mark { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Resource> Resources { get; set; }
    }
}
