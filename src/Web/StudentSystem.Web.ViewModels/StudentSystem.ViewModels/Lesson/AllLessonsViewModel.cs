namespace StudentSystem.ViewModels.Lesson
{
    using System;

    public class AllLessonsViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string CourseName { get; set; }

        public string Begining { get; set; }

        public string End { get; set; }

        //From automapper
        public string Date { get; set; }
    }
}
