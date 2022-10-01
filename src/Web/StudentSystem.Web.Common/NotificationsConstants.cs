namespace StudentSystem.Web.Common
{
    public static class NotificationsConstants
    {
        public const string SUCCESS_NOTIFICATION = "Success";
        public const string ERROR_NOTIFICATION = "Error";
        public const string WARNING_NOTIFICATION = "Warning";
        public const string INFO_NNOTIFICATION = "Info";

        // Courses messages

        public const string SUCCESSFULLY_CREATED_COURSE_MESSAGE = "Successfully created a {0} course";
        public const string SUCCESSFULLY_DELETE_COURSE_MESSAGE = "Successfully delete a course";
        public const string SUCCESSFULLY_UPDATE_COURSE_MESSAGE = "Successfully update a {0} course";
        public const string SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE = "You have successfully registered for the {0} course";
        public const string INVALID_COURSE_MESSAGE = "No such course exists";
        public const string ALREADY_IN_COURSE_MESSAGE = "You are already enrolled in this course";

        //Date message
        public const string SECOND_DATE_CANNOT_BE_EARLIER_MESSAGE = "{0} cannot be earlier than {1}";
        public const string START_DATE_MESSAGE = "The start date cannot be earlier {0}";

        //Lessons
        public const string SUCCESSFULLY_CREATED_LESSON_MESSAGE = "Successfully created a new lesson";
        public const string SUCCESSFULLY_UPDATE_LESSON_MESSAGE = "Successfully update a {0} lesson";
        public const string SUCCESSFULLY_DELETE_LESSON_MESSAGE = "Successfully delete a lesson";
        public const string INVALID_LESSON_MESSAGE = "No such lesson exists";

        //Reviw
        public const string NOT_ALLOWED_TO_ADD_A_REVIEW_MESSAGE
            = "To leave a review, you must be registered in the course";
    }
}
