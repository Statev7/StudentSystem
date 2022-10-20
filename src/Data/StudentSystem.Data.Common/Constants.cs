namespace StudentSystem.Data.Common
{
    public static class Constants
    {
        //Course

        public const int COURSE_NAME_MAX_LENGTH = 128;
        public const int COURSE_NAME_MIN_LENGTH = 2;

        public const int COURSE_DESCRIPTION_MAX_LENGTH = 10000;
        public const int COURSE_DESCRIPTION_MIN_LENGTH = 10;

        //Lesson

        public const int LESSON_TITLE_MAX_LENGTH = 64;
        public const int LESSON_TITLE_MIN_LENGTH = 3;

        public const int LESSON_CONTENT_MAX_LENGTH = 2000;
        public const int LESSON_CONTENT_MIN_LENGTH = 10;

        //Resource

        public const int RESOURCE_NAME_MAX_LENGTH = 128;
        public const int RESOURCE_NAME_MIN_LENGTH = 6;
        public const int RESOURCE_URL_MAX_LENGTH = 2048;

        // Review

        public const int REVIEW_CONTENT_MAX_LENGTH = 10000;

        // City

        public const int CITY_NAME_MAX_LENGTH = 128;

        // Category

        public const int CATEGORY_NAME_MAX_LENGTH = 32;

        // Users

        public const int FIRST_NAME_MAX_LENGTH = 64;
        public const int FIRST_NAME_MIN_LENGTH = 2;

        public const int LAST_NAME_MAX_LENGTH = 64;
        public const int LAST_NAME_MIN_LENGTH = 2;

        public const int PASSWORD_MAX_LENGTH = 100;
        public const int PASSWORD_MIN_LENGTH = 6;
    }
}
