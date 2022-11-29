namespace StudentSystem.Services.User.Models
{
    using System;

    public class UserDetailsServiceModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public string CityName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
