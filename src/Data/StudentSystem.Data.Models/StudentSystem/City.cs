namespace StudentSystem.Data.Models.StudentSystem
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class City : BaseModel
    {
        public City()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(CITY_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
