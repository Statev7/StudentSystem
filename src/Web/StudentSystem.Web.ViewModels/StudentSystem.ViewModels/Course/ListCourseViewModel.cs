namespace StudentSystem.ViewModels.Course
{
    using System;

    public class ListCourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        //From automapper.
        public double Duration { get; set; }
    }
}
