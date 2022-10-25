namespace StudentSystem.ViewModels.Course
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Category;
    using StudentSystem.ViewModels.Page;

    public class AllCoursesViewModel : PageViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }

        public IEnumerable<CategoryIdNameViewModel> Categories { get; set; }
    }
}
