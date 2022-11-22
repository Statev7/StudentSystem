namespace StudentSystem.ViewModels.Course
{
    using System;

    public class OpenCourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        public double Duration { get; set; }

        public bool IsDeleted { get; set; }
    }
}
