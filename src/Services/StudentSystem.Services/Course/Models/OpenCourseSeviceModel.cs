namespace StudentSystem.Services.Course.Models
{
    using System;

    public class OpenCourseSeviceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        public double Duration { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
