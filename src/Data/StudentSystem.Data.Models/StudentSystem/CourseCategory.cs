namespace StudentSystem.Data.Models.StudentSystem
{
    using Data.Models.Abstraction;

    public class CourseCategory : BaseModel
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
