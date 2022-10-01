namespace StudentSystem.Services.Course.Models
{
	using System.Collections.Generic;

	using StudentSystem.Data.Models.StudentSystem;

    public class CourseUsersServiceModel
	{
		public string Name { get; set; }

		public IEnumerable<UserCourse> UserCourses { get; set; }
	}
}
