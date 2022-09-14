namespace StudentSystem.ViewModels.Course
{
    using System;

    public class AllCoursesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Duration
            => this.CalculateDuration();

        private double CalculateDuration()
            => (EndDate - StartDate).TotalDays / 7 < 1 ? 1 : Math.Ceiling((EndDate - StartDate).TotalDays / 7);
    }
}
