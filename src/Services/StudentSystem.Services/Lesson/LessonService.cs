namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;
    using StudentSystem.Data.Models.StudentSystem;

    public class LessonService : BaseService, ILessonService
    {
        public LessonService(
            StudentSystemDbContext dbContext, 
            IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public IEnumerable<TEntity> GetAll<TEntity>()
            => this.DbContext.Lessons
                .Where(l => !l.IsDeleted)
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToList();

        public TEntity GetById<TEntity>(int id)
        {
            var lesson = this.DbContext.Lessons.Find(id);

            var lessonToReturn = this.Mapper.Map<TEntity>(lesson);
            return lessonToReturn;
        }      

        public async Task CreateAsync(CreateLessonBindingModel lesson)
        {
            var lessonToAdd = this.Mapper.Map<Lesson>(lesson);

            await this.DbContext.AddAsync(lessonToAdd);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
