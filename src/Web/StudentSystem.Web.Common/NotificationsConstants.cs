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
        public const string SUCCESSFULLY_DELETE_COURSE_MESSAGE = "Successfully delete a {0} course";
        public const string INVALID_COURSE_MESSAGE = "No such course exists";

        //Date message
        public const string INVALID_DATE_MESSAGE = "{0} cannot be earlier than {1}";
    }
}
