namespace StudentSystem.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class StudentSystemDbContext : IdentityDbContext
    {
        public StudentSystemDbContext(DbContextOptions<StudentSystemDbContext> options)
            : base(options)
        {
        }
    }
}
