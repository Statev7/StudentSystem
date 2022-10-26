namespace StudentSystem.ViewModels.Course
{
    using System;
    using System.Collections.Generic;

    public class CourseViewModel 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        //From automapper.
        public double Duration { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<int> CategoriesIds { get; set; }
    }
}
