namespace StudentSystem.Web.Infrastructure.MappingConfiguration
{
    using System;
    using System.Globalization;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            this.CreateMap<Course, AllCoursesViewModel>()
                .ForMember(d => d.StartDate, conf => conf
                    .MapFrom(s => s.StartDate.ToString("d-MMMM yyyy", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Duration, conf => conf
                    .MapFrom(s => (s.EndDate - s.StartDate).TotalDays / 7 < 1 
                            ? 1 
                            : Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)));

            this.CreateMap<CreateCourseBindingModel, Course>();
            this.CreateMap<Course, DetailCourseViewModel>();
            this.CreateMap<UpdateCourseBindingModel, Course>().ReverseMap();
            this.CreateMap<Course, CourseIdNameViewModel>();
        }
    }
}
