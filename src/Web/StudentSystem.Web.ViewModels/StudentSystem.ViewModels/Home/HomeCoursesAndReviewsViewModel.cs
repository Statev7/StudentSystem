namespace StudentSystem.ViewModels.Home
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Review;

    public class HomeCoursesAndReviewsViewModel
    {
        public IEnumerable<OpenCourseViewModel> OpenCourses { get; set; }

        public IEnumerable<ReviewForHomeViewModel> Reviews { get; set; }

        public OpenCourseViewModel NewestCourse { get; set; }

        public IList<int> CategoriesIds { get; set; }
    }
}
