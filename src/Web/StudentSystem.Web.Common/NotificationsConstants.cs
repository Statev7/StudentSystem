namespace StudentSystem.Web.Common
{
    public static class NotificationsConstants
    {
        public const string SUCCESS_NOTIFICATION = "Success";
        public const string ERROR_NOTIFICATION = "Error";
        public const string WARNING_NOTIFICATION = "Warning";
        public const string INFO_NNOTIFICATION = "Info";

        // Course

        public const string SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE = "You have successfully registered for the {0} course";
        public const string ALREADY_IN_COURSE_MESSAGE = "You are already enrolled in this course";

        // Lesson
        public const string DATES_CANNOT_BE_EARLIER_OR_LATER_THAN_COURSE_DATES_MESSAGE 
            = "Lesson {0}/{1} cannot be earlier/later than the course {2}/{3}";

        //Date 
        public const string THE_START_TIME_CANNOT_BE_LATER_THAN_THE_END_TIME = "{0} cannot be later than {1}!";
        public const string START_DATE_MESSAGE = "The start date cannot be earlier than {0}!";

        //Review
        public const string NOT_HAVE_PERMISSION_MESSAGE = "You do not have permission to {0} this review";
        public const string INVALID_REVIEW_MESSAGE = "No such review exists";
        public const string NOT_ALLOWED_TO_ADD_A_REVIEW_MESSAGE
            = "To leave a review, you must be registered in the course";

        //Admin panel 
        public const string USER_NOT_EXIST_MESSAGE = "Such a user not exist";
        public const string SUCCESSFULLY_PROMOTED_MESSAGE = "Successfully promoted a user";
        public const string SUCCESSFULLY_DEMOTE_MESSSAGE = "Successfully demote a user";
        public const string SUCCESSFULLY_BAN_A_USER = "Successfully ban a user";
        public const string SUCCESSSFULLY_UNBAN_A_USER = "Successfully unban a user";
        public const string UNSUCCESSFULLY_PROMOTED_MESSAGE = "This user is already moderator!";
        public const string UNSUCCESSFULYY_DEMOTE_MESSAGE = "There is no lower rank than 'user'";
        public const string USER_IS_ALREADY_BANED_MESSAGE = "This user is already banned!";
        public const string USER_IS_NOT_BANNED_MESSAGE = "This user is not banned!";
        public const string CANNOT_CHANGE_OWN_ROLES_MESSAGE = "You cannot change your roles";
        public const string CANNOT_BAN_ADMIN_MESSAGE = "Cannot ban user with admin role!";
        public const string CANNOT_PROMOTE_A_BANNED_USER = "Cannot promote a banned user!";
        public const string CANNOT_DEMOTE_A_BANNED_USER = "Cannot demote a banned user!";

        //CRUD

        //{0} is name of entity, {1} is keyword (like course, lesson, ect)
        public const string SUCCESSFULLY_CREATED_ENTITY_MESSAGE = "Successfully created a {0} {1}!";
        public const string SUCCESSFULLY_UPDATED_ENTITY_MESSAGE = "Successfully update a {0} {1}!";
        public const string SUCCESSFULLY_DELETED_ENTITY_MESSAGE = "Successfully deleted!";

        //{0} is Keyword
        public const string SUCH_A_ENTITY_DOES_NOT_EXIST = "No such {0} exists";

        //Keywords
        public const string COURSE_KEYWORD = "course";
        public const string LESSON_KEYWORD = "lesson";
        public const string RESOURCE_KEYWORD = "resource";
    }
}
