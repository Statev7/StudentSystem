namespace StudentSystem.ViewModels.User
{
    using System;

    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CityName { get; set; }

        public string RoleName { get; set; }
    }
}
