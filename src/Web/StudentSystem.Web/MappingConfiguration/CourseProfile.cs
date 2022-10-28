namespace StudentSystem.Web.MappingConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            this.CreateMap<Course, CourseViewModel>()
                .ForMember(d => d.CategoriesIds, conf => conf
                    .MapFrom(s => s.CourseCategories.Select(cc => cc.CategoryId)))
                .ForMember(d => d.Duration, conf => conf
                    .MapFrom(s => (s.EndDate - s.StartDate).TotalDays / 7 < 1 
                            ? 1 
                            : Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)));

            this.CreateMap<Course, DetailCourseViewModel>();
            this.CreateMap<Course, CourseFormServiceModel>()
                .ForMember(d => d.CategoriesIds, conf => conf.MapFrom(s => s.CourseCategories.Select(cc => cc.CategoryId)))
                .ReverseMap();
            this.CreateMap<Course, CourseIdNameViewModel>();
            this.CreateMap<CourseLessonScheduleServiceModel, CourseIdNameViewModel>();
            this.CreateMap<Course, CourseUsersServiceModel>();
            this.CreateMap<Course, OpenCourseViewModel>()
                .ForMember(d => d.Duration, conf => conf
                    .MapFrom(s => (s.EndDate - s.StartDate).TotalDays / 7 < 1
                            ? 1
                            : Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)));
            this.CreateMap<Course, CourseNameViewModel>();
        }
    }
}
