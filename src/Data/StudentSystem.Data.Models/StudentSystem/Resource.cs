namespace StudentSystem.Data.Models.StudentSystem
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Resource : BaseModel
    {
        [Required]
        [MaxLength(RESOURCE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MaxLength(RESOURCE_URL_MAX_LENGTH)]
        public string URL { get; set; }

        public int? LessonId { get; set; }

        public Lesson Lesson { get; set; }
    }
}
