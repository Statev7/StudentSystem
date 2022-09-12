namespace StudentSystem.Data.EntityTypeConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.StudentSystem;

    public class LessonEntityTypeConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder
                .HasMany(l => l.Resources)
                .WithOne(r => r.Lesson)
                .HasForeignKey(r => r.LessonId);
        }
    }
}
