namespace StudentSystem.ViewModels.Resource
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class CreateResourceBindingModel
    {
        [Required]
        [MaxLength(RESOURCE_NAME_MAX_LENGTH)]
        [MinLength(RESOURCE_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string URL { get; set; }
    }
}
