namespace StudentSystem.Data.EntityTypeConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.StudentSystem;

    internal class ExamEntityTypeConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder
                .HasMany(e => e.Resources)
                .WithOne(r => r.Exam)
                .HasForeignKey(r => r.ExamId);

            builder
                .HasOne(e => e.Course)
                .WithOne(c => c.Exam)
                .HasForeignKey<Exam>(e => e.CourseId);
        }
    }
}
