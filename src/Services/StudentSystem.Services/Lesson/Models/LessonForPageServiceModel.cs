namespace StudentSystem.Services.Lesson.Models
{
	public class LessonForPageServiceModel
	{
        public int Id { get; set; }

        public string Title { get; set; }

        //For filter
        public int CourseId { get; set; }
    }
}
