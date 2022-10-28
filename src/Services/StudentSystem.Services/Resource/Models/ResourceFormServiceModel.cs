namespace StudentSystem.Services.Resource.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.Lesson;

    using static Data.Common.Constants;

    public class ResourceFormServiceModel
    {
        [Required]
        [MaxLength(RESOURCE_NAME_MAX_LENGTH)]
        [MinLength(RESOURCE_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string URL { get; set; }

        public int LessonId { get; set; }

        public IEnumerable<LessonIdNameViewModel> Lessons { get; set; }
    }
}
