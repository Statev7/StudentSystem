namespace StudentSystem.ViewModels.Resource
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class CreateResourceBindingModel
    {
        [Required]
        [MaxLength(ResourceNameMaxLength)]
        [MinLength(ResourceNameMinLength)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string URL { get; set; }
    }
}
