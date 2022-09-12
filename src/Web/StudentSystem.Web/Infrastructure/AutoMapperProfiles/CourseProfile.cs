﻿namespace StudentSystem.Web.Infrastructure.AutoMapperProfiles
{
    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            this.CreateMap<Course, AllCoursesViewModel>();
            this.CreateMap<CreateCourseBindingModel, Course>();
        }
    }
}
