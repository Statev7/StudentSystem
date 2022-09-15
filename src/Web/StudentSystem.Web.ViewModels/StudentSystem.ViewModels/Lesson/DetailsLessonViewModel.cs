namespace StudentSystem.ViewModels.Lesson
{
    public class DetailsLessonViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Begining { get; set; }

        public string End { get; set; }

        //From automapper
        public string Date { get; set; }
    }
}
