namespace StudentSystem.Data.Models.StudentSystem
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Resource : BaseModel<int>, IDeletableEntity
    {
        [Required]
        [MaxLength(ResourceNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ResourceURLMaxLength)]
        public string URL { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public int? ExamId { get; set; }

        public Exam Exam { get; set; }
    }
}
