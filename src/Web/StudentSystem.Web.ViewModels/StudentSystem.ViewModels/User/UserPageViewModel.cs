namespace StudentSystem.ViewModels.User
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Page;

    public class UserPageViewModel : PageViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
