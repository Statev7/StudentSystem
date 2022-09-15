namespace StudentSystem.Web.Infrastructure.MappingConfiguration
{
    using System.Globalization;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.Lesson;

    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            this.CreateMap<Lesson, CreateLessonBindingModel>().ReverseMap();

            this.CreateMap<Lesson, AllLessonsViewModel>()
                .ForMember(d => d.CourseName, conf => conf.MapFrom(s => s.Course.Name))
                .ForMember(d => d.Begining, conf => conf.MapFrom(s => s.Begining.ToString("HH:mm", CultureInfo.InvariantCulture)))
                .ForMember(d => d.End, conf => conf.MapFrom(s => s.End.ToString("HH:mm", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Date, conf => conf.MapFrom(s => s.Begining.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<Lesson, DetailsLessonViewModel>();
            this.CreateMap<Lesson, LessonIdNameViewModel>();

        }
    }
}
