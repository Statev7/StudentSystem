namespace StudentSystem.Data.Models.StudentSystem
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Category : BaseModel
    {
        public Category()
        {
            this.CourseCategories = new HashSet<CourseCategory>();
        }

        [MaxLength(CATEGORY_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public ICollection<CourseCategory> CourseCategories { get; set; }
    }
}
