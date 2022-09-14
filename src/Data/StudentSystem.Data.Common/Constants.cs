namespace StudentSystem.Data.Common
{
    public static class Constants
    {
        //Course

        public const int CourseNameMaxLength = 128;
        public const int CourseNameMinLength = 2;

        public const int CourseDescriptionMaxLength = 10000;
        public const int CourseDescriptionMinLength = 10;

        //Exam

        public const int ExamNameMaxLength = 128;

        //Lesson

        public const int LessonTitleMaxLength = 64;
        public const int LessonTitleMinLength = 3;

        public const int LessonContentMaxLength = 2000;
        public const int LessonContentMinLength = 10;

        //Resource

        public const int ResourceNameMaxLength = 128;
        public const int ResourceURLMaxLength = 2048;
    }
}
