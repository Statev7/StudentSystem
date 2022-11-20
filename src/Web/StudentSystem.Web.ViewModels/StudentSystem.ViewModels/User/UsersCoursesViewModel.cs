namespace StudentSystem.ViewModels.User
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Course;

    public class UsersCoursesViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<CourseIdNameViewModel> Courses { get; set; }
    }
}
