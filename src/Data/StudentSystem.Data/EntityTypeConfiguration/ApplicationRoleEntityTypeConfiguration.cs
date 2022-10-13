namespace StudentSystem.Data.EntityTypeConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.StudentSystem;

    public class ApplicationRoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasMany(e => e.UserRoles)
                 .WithOne(e => e.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
        }
    }
}
