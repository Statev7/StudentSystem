namespace StudentSystem.Data.Models.StudentSystem
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
            :this(null)
        {
            this.UserRoles = new HashSet<ApplicationUserRole>();
        }

        public ApplicationRole(string roleName)
            :base(roleName)
        {
        }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
