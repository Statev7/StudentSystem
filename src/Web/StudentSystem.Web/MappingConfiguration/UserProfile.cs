﻿namespace StudentSystem.Web.MappingConfiguration
{
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Services.User.Models;
    using StudentSystem.ViewModels.User;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(d => d.IsUserBanned, conf => conf.MapFrom(s => s.IsDeleted))
                .ForMember(d => d.RoleName, conf => conf.MapFrom(s => s.UserRoles
                    .Select(ur => ur.Role.Name)
                    .FirstOrDefault()));

            this.CreateMap<ApplicationUser, UsersExportServiceModel>()
                .ForMember(d => d.IsBanned, conf => conf.MapFrom(s => s.IsDeleted))
                .ForMember(d => d.CityName, conf => conf
                    .MapFrom(s => s.City.Name ?? "No information"));

            this.CreateMap<ApplicationUser, UpdateUserServiceModel>().ReverseMap();
            this.CreateMap<ApplicationUser, CreateUserServiceModel>().ReverseMap();
            this.CreateMap<ApplicationUser, UserDetailsServiceModel>();
        }
    }
}
