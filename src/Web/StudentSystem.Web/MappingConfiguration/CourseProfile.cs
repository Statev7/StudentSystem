namespace StudentSystem.Web.MappingConfiguration
{
    using System;
    using System.Globalization;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            this.CreateMap<Course, ListCoursesViewModel>()
                .ForMember(d => d.Duration, conf => conf
                    .MapFrom(s => (s.EndDate - s.StartDate).TotalDays / 7 < 1 
                            ? 1 
                            : Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)));

            this.CreateMap<Course, DetailCourseViewModel>();
            this.CreateMap<Course, CourseFormServiceModel>().ReverseMap();
            this.CreateMap<Course, CourseIdNameViewModel>();
            this.CreateMap<CourseLessonScheduleServiceModel, CourseIdNameViewModel>();
        }
    }
}
