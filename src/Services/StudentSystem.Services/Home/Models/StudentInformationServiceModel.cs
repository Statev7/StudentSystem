namespace StudentSystem.Services.Home.Models
{
    using System.Collections.Generic;

    using StudentSystem.Services.Course.Models;

    public  class StudentInformationServiceModel
    {
        public IEnumerable<CourseLessonScheduleServiceModel> Courses { get; set; }
    }
}
