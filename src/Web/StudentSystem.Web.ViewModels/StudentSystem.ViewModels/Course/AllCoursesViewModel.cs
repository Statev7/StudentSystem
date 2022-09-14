namespace StudentSystem.ViewModels.Course
{
    using System;

    public class AllCoursesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public string StartDate { get; set; }

        //From automapper.
        public double Duration { get; set; }
    }
}
