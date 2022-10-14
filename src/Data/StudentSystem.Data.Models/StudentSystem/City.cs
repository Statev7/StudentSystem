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
        [MaxLength(CityNameMaxLength)]
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
